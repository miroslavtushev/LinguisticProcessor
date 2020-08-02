using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SEEL.LinguisticProcessor.Exceptions;
using SEEL.LinguisticProcessor.Spellcheck;
using SEEL.LinguisticProcessor.Util;

namespace SEEL.LinguisticProcessor.NamesExtractors
{
    public class BaseNamesExtractor : INotifyPropertyChanged
    {
        
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged implementation

        #region Properties
        /// <summary>
        /// Determines the regular expression to be used depending on a language
        /// </summary>
        protected Regex RegularExpression { get; set; }

        /// <summary>
        /// Determines the target language
        /// </summary>
        protected string TargetLanguage { get; set; }

        protected static readonly HashSet<string> EnglishStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        protected static readonly HashSet<string> NaturalLanguageWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        protected static readonly Dictionary<string, int> NaturalLanguageWordsFreq = new Dictionary<string, int>();

        /// <summary>
        /// The message to display on the status bar GUI
        /// </summary>
        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsPorterStemmer { get; set; }
        public bool IsCamelCase { get; set; }
        public bool IsUnderscore { get; set; }
        public bool IsOutputProgressForFiles { get; set; }

        public List<string> Releases { get; set; } = new List<string>();

        public string CurrentProjectPath { get; set; }
        public string SelectedOutputForProject { get; set; }
        protected static readonly List<Splitters.ISplitter> Splitters;
        protected static readonly Stemmers.PorterStemmer Stemmer = new Stemmers.PorterStemmer();
        #endregion

        public BaseNamesExtractor(string pathToProject)
        {
            CurrentProjectPath = pathToProject;
        }

        static BaseNamesExtractor()
        {
            #region Loading resources
            // get the English Stop Words
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName1 = "SEEL.LinguisticProcessor.Resources.EnglishStopWords.txt";
            var resourceName2 = "SEEL.LinguisticProcessor.Resources.NaturalLanguageWordsByFrequency.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName1))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    EnglishStopWords.Add(line);
                }
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName2))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    NaturalLanguageWords.Add(line);
                }
            }
            #endregion
            Splitters = new List<Splitters.ISplitter>
            {
                new Splitters.CommaSplitter(),
                new Splitters.DotSplitter(),
                new Splitters.DigitSplitter()
            };
        }


        /// <summary>
        /// Runs the program
        /// </summary>
        public async virtual Task Run()
        {
            if (IsCamelCase) Splitters.Insert(0, new Splitters.CamelCaseSplitter());
            if (IsUnderscore) Splitters.Insert(3, new Splitters.UnderscoreSplitter());

            string[] allReleases;
            try
            {
                allReleases = HelperFunctions.FindReleases(CurrentProjectPath);
            }
            catch (NoReleasesFoundException)
            {
                Message = "No releases were found";
                return;
            }
            foreach (var release in allReleases)
            {
                Message = $@"Processing {release}...";
                string[] allSourceFiles = null;
                allSourceFiles = FindSourceFiles($@"{release}");
                if (allSourceFiles == null || allSourceFiles.Length == 0)
                {
                    Message = $@"No source files were found for {release}: continuing...";
                    continue;
                }
                Regex re = new Regex(@"\\release(\d+)$");
                Match match = re.Match(release);

                await Task.Run(() => ProcessSourceFiles(allSourceFiles, match.Groups[1].Value));
            }
        }

        protected virtual string[] FindSourceFiles(string pathToFolder)
        {
            string[] sourceFiles = Directory.GetFiles(pathToFolder, "*" + TargetLanguage, SearchOption.TopDirectoryOnly);
            Message = $@"Found {sourceFiles.Length} source files";
            return sourceFiles;
        }

        protected virtual void ProcessSourceFiles(string[] allSourceFiles, string releaseNumber)
        {
            // we need to have a separate dictionary for each release
            var WordsDic = new Dictionary<Word, int>();
            var MisspellingsDic = new Dictionary<Word, int>();
            var AcronymsDic = new Dictionary<Word, int>();

            foreach (var file in allSourceFiles)
            {
                Message = $@"Processing {file}...";
                string sourceFile = null;
                try
                {
                    sourceFile = File.ReadAllText(file);
                }
                catch
                {
                    Message = $@"An error occured while reading the file: {file}";
                    throw;
                }

                // Step 1. Initial word extraction
                ExtractWords(sourceFile, WordsDic, MisspellingsDic, AcronymsDic);
            }

            // Step 2. Several natural words concatenated together. Splitting with no delimiter. Returned unidentified words
            var ret = Step2(WordsDic, MisspellingsDic);
            // Step 3. Several natural words with abbreviation prefix. Returned words with prefixes removed
            var ret2 = Step3(AcronymsDic, ret);
            // Step 4. Remove first 4 chars one by one and check if the resulting word is a natural word
            var ret3 = Step4(WordsDic, AcronymsDic, ret2);

            MisspellingsDic = new Dictionary<Word, int>();
            var UnidentifiedDic = new Dictionary<Word, int>();

            // optimization
            var NatLangWordsList = NaturalLanguageWords.ToList();
            Parallel.ForEach(ret3, word =>
            {
               if (IsMisspelling(word.Key.Name, NatLangWordsList))
                   MisspellingsDic.TryAdd(word.Key, word.Value);
               else
                   UnidentifiedDic.TryAdd(word.Key, word.Value);
           });

            PrepareResults(releaseNumber, new DictionaryWrapper(WordsDic, "words"),
                                          new DictionaryWrapper(UnidentifiedDic, "unidentified"),
                                          new DictionaryWrapper(MisspellingsDic, "misspellings"),
                                          new DictionaryWrapper(AcronymsDic, "acronyms"));
            Releases.Add("release" + releaseNumber);         
        }

        private void PrepareResults(string releaseNumber, params DictionaryWrapper[] list)
        {
            foreach (var d in list)
            {
                var q = from pair in d.Dict
                        orderby pair.Value descending
                        select pair;
                using (StreamWriter fcomm = new StreamWriter($@"{CurrentProjectPath}\r{releaseNumber}_{d.Name}_comments.txt", false))
                using (StreamWriter fcode = new StreamWriter($@"{CurrentProjectPath}\r{releaseNumber}_{d.Name}_code.txt", false))
                {
                    Message = $@"Saving code results for release{releaseNumber}...";
                    foreach (var pair in q)
                    {
                        if (pair.Key.Type == 'S')
                            fcode.WriteLine($@"{pair.Key.Name} {pair.Value}");
                        else
                            fcomm.WriteLine($@"{pair.Key.Name} {pair.Value}");
                    }
                }
            }
        }

        protected void ExtractWords(string src, Dictionary<Word, int> WordsDic, Dictionary<Word, int> MisspellingsDic, Dictionary<Word, int> AcronymsDic)
        {
            Match match = RegularExpression.Match(src);
            // for matching words in the comment (alphanumeric only)
            Regex commentRegex = new Regex(@"[a-zA-Z][a-zA-Z1-9]*", RegexOptions.Compiled);
            while (match.Success)
            {
                if (match.Groups["commentSingle"].Success)
                {
                    // STEP 1. Tokenize
                    var input = LanguageSpecificStep(match.Groups["commentSingle"].Value);
                    MatchCollection commentMatch = commentRegex.Matches(input);
                    foreach (Match val in commentMatch)
                    {
                        // STEP 2. Remove English stop words
                        var result = ApplySplitters(val.Value);
                        foreach (var res in result)
                        {
                            if (!IsEnglishStopWord(res))
                            {
                                if (IsNaturalWord(res)) WordsDic.TryAdd(new Word(Stemmer.StemWord(res), 'C'));
                                else if (IsAbbreviation(res, AcronymsDic.Keys.ToList())) AcronymsDic.TryAdd(new Word(res, 'C'));
                                else MisspellingsDic.TryAdd(new Word(res, 'C'));
                            } 
                        }
                    }
                }
                else if (match.Groups["commentMulti"].Success)
                {
                    // STEP 1. Tokenize
                    MatchCollection commentMatch = commentRegex.Matches(match.Groups["commentMulti"].Value);
                    foreach (Match val in commentMatch)
                    {
                       
                        // STEP 2. Remove English stop words
                        var result = ApplySplitters(val.Value);
                        foreach (var res in result)
                        {
                            if (!IsEnglishStopWord(res))
                            {
                                if (IsNaturalWord(res)) WordsDic.TryAdd(new Word(Stemmer.StemWord(res), 'C'));
                                else if (IsAbbreviation(res, AcronymsDic.Keys.ToList())) AcronymsDic.TryAdd(new Word(res, 'C'));
                                else MisspellingsDic.TryAdd(new Word(res, 'C'));
                            }
                        }
                    }

                }
                else if (match.Groups["string"].Success)
                {
                    match = match.NextMatch();
                    continue;
                }
                else if (match.Groups["hexnum"].Success)
                {
                    match = match.NextMatch();
                    continue;
                }
                else if (match.Groups["annotation"].Success)
                {
                    match = match.NextMatch();
                    continue;
                }
                
                else if (match.Groups["identifier"].Success)
                {
                    // STEP 1. Remove keywords
                    if (IsKeyword(match.Groups["identifier"].Value))
                    {
                        match = match.NextMatch();
                        continue;
                    }

                    // STEP 2. Split
                    var result = ApplySplitters(match.Groups["identifier"].Value);
                    foreach (var res in result)
                    {
                        if (!IsEnglishStopWord(res))
                        {
                            if (IsNaturalWord(res)) WordsDic.TryAdd(new Word(Stemmer.StemWord(res)));
                            else if (IsAbbreviation(res, AcronymsDic.Keys.ToList())) AcronymsDic.TryAdd(new Word(res));
                            else MisspellingsDic.TryAdd(new Word(res));
                        }
                    }
                }
                match = match.NextMatch();
            }
        }

        private Dictionary<Word, int> Step2(Dictionary<Word, int> WordsDic, Dictionary<Word, int> MisspellingsDic)
        {
            var unidentified = new Dictionary<Word, int>();
            foreach (var word in MisspellingsDic)
            {
                string res;
                if ((word.Key.Name[word.Key.Name.Length - 1] == 's' && word.Key.Name[word.Key.Name.Length - 2] != 's') || !char.IsLetter(word.Key.Name[word.Key.Name.Length - 1]))
                    res = new Splitters.NoDelimiterSplitter().Split(word.Key.Name.Remove(word.Key.Name.Length - 1));
                else
                    res = new Splitters.NoDelimiterSplitter().Split(word.Key.Name);
                var t = res.Split(' ');
                // if it's several words from existing word dictionary concatenated together
                if (t.Length < 4 && Array.TrueForAll(t, elem => elem.Length > 1) && Array.TrueForAll(t, elem => IsNaturalWord(elem)))
                {
                    foreach (var r in t)
                    {
                        if (!IsEnglishStopWord(r))
                            WordsDic.TryAdd(new Word(Stemmer.StemWord(r), word.Key.Type), word.Value);
                    }
                }
                else
                {
                    unidentified.TryAdd(new Word(word.Key.Name, word.Key.Type), word.Value);
                }
            }
            return unidentified;
        }

        private Dictionary<Word, int> Step3(Dictionary<Word, int> AcronymsDic, Dictionary<Word, int> MisspellingsDic)
        {
            var unidentified = new Dictionary<Word, int>();
            foreach (var word in MisspellingsDic)
            {            
                var possiblePrefxs = new List<string>();
                foreach (var acr in AcronymsDic)
                {
                    if (word.Key.Name.StartsWith(acr.Key.Name))
                        possiblePrefxs.Add(acr.Key.Name);
                }
                string res = word.Key.Name;
                // if we found possible prefix - replace with the longest one
                if (possiblePrefxs.Count != 0)
                {
                    var replacement = possiblePrefxs.Aggregate(string.Empty, (seed, f) => f.Length > seed.Length ? f : seed);
                    res = word.Key.Name.Replace(replacement, "");
                    // add prefix to the dictionary
                    AcronymsDic.TryAdd(new Word(replacement, word.Key.Type), word.Value);  
                }
                // add word to return dic. With or without prefix
                if (res.Length == 1)
                    continue;
                unidentified.TryAdd(new Word(res, word.Key.Type), word.Value);
            }
            return unidentified;
        }

        private Dictionary<Word, int> Step4(Dictionary<Word, int> WordsDic, Dictionary<Word, int> AcronymsDic, Dictionary<Word, int> MisspellingsDic)
        {
            var unidentified = new Dictionary<Word, int>();
            foreach (var word in MisspellingsDic)
            {
                for (int i = 1; i <= 4; i++)
                {
                    string r;
                    try
                    {
                        r = word.Key.Name.Substring(i);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    if (IsNaturalWord(r))
                    {
                        if (!IsEnglishStopWord(r))
                            WordsDic.TryAdd(new Word(Stemmer.StemWord(r), word.Key.Type), word.Value);

                        AcronymsDic.TryAdd(new Word(word.Key.Name.Substring(0, i), word.Key.Type), word.Value);      
                        break;
                    }
                    if (i == 4) // if we did 4 iterations without break
                    {
                        unidentified.TryAdd(word.Key, word.Value);
                    }
                } 
            }
            return unidentified;
        }

        private bool IsEnglishStopWord(string val) => EnglishStopWords.Contains(val.ToLower());
        
        private bool IsNaturalWord(string val) => NaturalLanguageWords.Contains(val.ToLower());

        private string[] ApplySplitters(string input)
        {
            string result = input;
            foreach (var splitter in Splitters)
                result =  splitter.Split(result);
            return result.Split(' ').Where( x => x != " ").ToArray();
        }

        private bool IsMisspelling(string word, List<string> words, int maxAllowedSymbals = 2)
        {
            // Identifying misspellings with maximum allowed misspelled symbols = 2
            var ld = new DamerauLevenshteinDistance() { MaxAllowedDistance = maxAllowedSymbals };
            foreach (var w in words)
            {
                if (ld.IsMisspelling(word, w))
                    return true;
            }
            return false;
        }

        private bool IsAbbreviation(string word, List<Word> words)
        {
            if (word.Length <= 4) return true;
            foreach (var w in words)
            {
                bool res;
                try
                {
                    res = Regex.IsMatch(w.Name, $@"^{word}");
                }
                catch (Exception)
                {
                    return false;
                }
                if (res)
                    return true;
            }
            return false;
        }

        protected virtual bool IsKeyword(string ident) => throw new NotImplementedException();
        protected virtual string LanguageSpecificStep(string input) => input;
    }
}
