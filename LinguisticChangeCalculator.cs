using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SEEL.LinguisticProcessor;
using SEEL.LinguisticProcessor.Util;

namespace Program
{
    public class LinguisticChangeCalculator : INotifyPropertyChanged
	{
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged implementation

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


		public static double WordUtility (Dictionary<string, int> listOfWords, string word)
        {
			if (listOfWords.TryGetValue(word, out int value))
			{
				return Convert.ToDouble(value) / Convert.ToDouble(listOfWords.Sum(x => x.Value));
			}
			else
			{
				return 0D;
			}
		}

        /// <summary>
        /// Calculates the utility for all words in the file an saves it
        /// </summary>
        /// <param name="pathToFile">Path to a txt file with words and frequencies</param>
        /// <returns></returns>
        public static void WordsUtility(string pathToFile)
        {
            Dictionary<string, int> wordsDic = new Dictionary<string, int>();

            try
            {
                using (StreamReader file = new StreamReader(pathToFile))
                {
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        // the input in the form of "variableName 2432"
                        string[] splitLine = line.Split(null);
                        wordsDic.Add(splitLine[0], Int32.Parse(splitLine[1]));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($@"There was an error reading the file: {pathToFile}");
                Constants.log.Error(e.Message);
                Environment.Exit(1);
            }

            // extracting file name
            string outFile = Regex.Split(pathToFile, @"\.txt")[0] + "Ling.txt";
            // calculating sum of freqs
            decimal sumOfFreqs = wordsDic.Sum(x => x.Value);

            // calculating utility for each word and writing results to a file
            try
            {
                using (StreamWriter writer = new StreamWriter(outFile))
                {
                    foreach (var word in wordsDic.Keys)
                    {
                        decimal lingChange = 0m;
                        if (wordsDic.TryGetValue(word, out int frequency))
                        {
                            lingChange = Convert.ToDecimal(frequency) / sumOfFreqs;
                        }
                        writer.Write($@"{word} {frequency} {lingChange}");
                        writer.Write(Environment.NewLine);
                        writer.Flush();
                    }  
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($@"There was an error writing to the file: {outFile}");
                Constants.log.Error(e.Message);
                Environment.Exit(1);
            }

        }

        /// <summary>
        /// Calculates the word utility for all words (code + comments) and writes them into files
        /// </summary>
        /// <param name="pathToProject">Path to project</param>
        /// <returns></returns>
        public void WordUtilityCodeCommentsWordsToFile(string pathToProject)
        {
            // reading the release file
            if (!pathToProject.EndsWith(@"\"))
            {
                pathToProject = pathToProject + @"\";
            }

            // get all code.txt and comment.txt files
            List<string> allTxtFiles = Directory.GetFiles(pathToProject, "*.txt").ToList();
            List<string> codeCommentFiles = new List<string>();
            foreach (var file in allTxtFiles)
            {
                if (Regex.IsMatch(file, @"(code|comments)\d+\.txt", RegexOptions.Compiled))
                {
                    codeCommentFiles.Add(file);
                }
            }

            // process them one by one
            foreach (var txtFile in codeCommentFiles)
            {
                Message = $"Processing {txtFile}...";
                WordsUtility(txtFile);
            }
        }

        public static double AverageWordUtility (List<Dictionary<string, int>> listOfReleasesWithWords, string word)
		{
            // calculate utility for each statFile
            double totalUtility = 0;
            int firstReleaseToAppear = 1;
            bool isInitial = true;
            try
            {
                if (listOfReleasesWithWords.Count == 0)
                    return 0;
                for (int i = 1; i <= listOfReleasesWithWords.Count; i++)
                {
                    double curUtility = WordUtility(listOfReleasesWithWords[i-1], word);

                    if (curUtility == 0 && isInitial) 
                        firstReleaseToAppear = i;
                    else if (curUtility != 0) 
                        isInitial = false;

                    totalUtility += curUtility;
                }
            }
            catch (Exception)
            {
                return 0;
            }

			return totalUtility / Convert.ToDouble(listOfReleasesWithWords.Count - firstReleaseToAppear + 1);
		}

        /// <summary>
        /// Calculates the logarithmic growth rate
        /// </summary>
        /// <returns>The growth rate.</returns>
        /// <param name="startValue">Initial value</param>
        /// <param name="endValue">End value</param>
        /// <param name="length">Length of growth period</param>
        public double LogarithmicGrowthRate (decimal startValue, decimal endValue, int length)
		{
			return Math.Log(Convert.ToDouble(endValue
                                             / startValue)) / Convert.ToDouble(length);
		}

        public double AverageSurvivalRate(string pathToProject, string nameOfFile, string word)
        {
            HelperFunctions.FindReleases(pathToProject, out List<string> statFiles, out int numOfReleases);

            // obtain the list of word appearance for each release
            List<bool> wordCounts = new List<bool>();
            for (int i = 1; i <= numOfReleases; i++)
            {
                string[] lines = File.ReadAllLines(pathToProject + @"\" + nameOfFile + i + ".txt");
                wordCounts.Add(false);
                foreach (var line in lines)
                {
                    if (Regex.IsMatch(line, $@"{word} \d+"))
                    {
                        wordCounts[i-1] = true;
                        break;
                    }
                }              
            }

            // find the release where the word was born
            int firstRelease = 1;
            while (firstRelease <= wordCounts.Count)
            {
                if (wordCounts[firstRelease - 1] != false) break;
                else firstRelease++;
            }

            // find the release where the word died
            int lastRelease = wordCounts.Count;
            while (lastRelease > firstRelease)
            {
                if (wordCounts[lastRelease - 1] == false) lastRelease--;
                else break;
            }

            // calculate % between those 2 releases
            return Convert.ToDouble(lastRelease - firstRelease + 1) / Convert.ToDouble(wordCounts.Count) * 100d;
        }

        public List<double> LinguisticChangeForAProject(string pathToProject)
        {
            // extract a list of all unique words for each category
            var words = GetUniqueWords(pathToProject, "words");
            var acronyms = GetUniqueWords(pathToProject, "acronyms");
            var misspellings = GetUniqueWords(pathToProject, "misspellings");

            // get a List<Dictionary> for all tokens in project
            var listOfReleasesWithWords = new List<Dictionary<string, int>>();
            var releases = HelperFunctions.FindReleases(pathToProject);
            for (int i = 0; i < releases.Length; i++)
            {
                var m = Regex.Matches(releases[i], @"\d+");
                var num = Int32.Parse(m[m.Count - 1].Value);
                listOfReleasesWithWords.Add(GetWordsForSpecificRelease(pathToProject, num));
            }

            // calculate ling. change for each consequtive release
            var lingChange = new List<double>();

            for (int i = 0; i < listOfReleasesWithWords.Count - 1; i++)
            {
                Message = $@"Calculating for release {i + 2}...";
                var ret = CalculateLinguisticChangeBetweenTwoReleases(listOfReleasesWithWords[i], listOfReleasesWithWords[i + 1]);
                lingChange.Add(ret);
            }
            return lingChange;
        }

        public double CalculateLinguisticChangeBetweenTwoReleases(Dictionary<string, int> release1, Dictionary<string, int> release2)
        {
            IEnumerable<string> intersect = release1.Keys.Intersect(release2.Keys);
            IEnumerable<string> union = release1.Keys.Union(release2.Keys);

            return 1d - Math.Abs((double)intersect.Count()) / Math.Abs((double)union.Count());
        }

        public void WriteLinguisticChangeForAProjectToFile(string pathToProject)
        {
            // prepare dataset
            Message = $@"Creating the dataset {pathToProject}\LinguisticChange.txt...";

            Message = $@"Processing {pathToProject}...";
            using (var dataSet = new StreamWriter($@"{pathToProject}\LinguisticChange.txt", true))
            {
                var lingChange = LinguisticChangeForAProject(pathToProject);

                try
                {
                    dataSet.Write("LinguisticChange");
                    dataSet.Write(Environment.NewLine);
                    for (int i = 0; i < lingChange.Count; i++)
                    {
                        dataSet.Write($@"{lingChange[i]}");
                        dataSet.Write(Environment.NewLine);
                    }
                    dataSet.Flush();
                }
                catch (Exception)
                {
                    dataSet.Flush();
                    throw;
                }
            }

            Message = $@"The results are saved in: {pathToProject}\LinguisticChange.txt";
        }

        /// <summary>
        /// Calculates the linguistic change between two releases
        /// </summary>
        /// <returns>The linguistic change between two releases</returns>
        /// <param name="pathToProject">Path to project</param>
        /// <param name="firstRelease">First release</param>
        /// <param name="secondRelease">Second release. Default = last release</param>
        /// <exception cref="Exception">Thrown when the file is not found</exception>
        public (decimal, decimal) LinguisticChangeBetweenTwoReleases(string pathToProject, int firstRelease, int secondRelease = 0)
        {
            List<string> release1code = new List<string>();
            List<string> release2code = new List<string>(); 
            List<string> release1comm = new List<string>();
            List<string> release2comm = new List<string>();

            if (secondRelease == 0)
            {
                HelperFunctions.FindReleases(pathToProject, out List<string> statFiles, out secondRelease);
            }

            if (!pathToProject.EndsWith(@"\"))
            {
                pathToProject = pathToProject + @"\";
            }

            var file1Lcode = new List<string>(new string[] {
            Path.Combine(pathToProject, $"r{firstRelease.ToString()}_words_code.txt"),
            Path.Combine(pathToProject, $"r{firstRelease.ToString()}_acronyms_code.txt"),
            Path.Combine(pathToProject, $"r{firstRelease.ToString()}_misspellings_code.txt"),
            Path.Combine(pathToProject, $"r{firstRelease.ToString()}_unidentified_code.txt"),
            });
            var file1Lcomm = new List<string>(new string[] {
            Path.Combine(pathToProject, $"r{firstRelease.ToString()}_words_comments.txt"),
            Path.Combine(pathToProject, $"r{firstRelease.ToString()}_acronyms_comments.txt"),
            Path.Combine(pathToProject, $"r{firstRelease.ToString()}_misspellings_comments.txt"),
            Path.Combine(pathToProject, $"r{firstRelease.ToString()}_unidentified_comments.txt"),
            });

            var file2Lcode = new List<string>(new string[] {
            Path.Combine(pathToProject, $"r{secondRelease.ToString()}_words_code.txt"),
            Path.Combine(pathToProject, $"r{secondRelease.ToString()}_acronyms_code.txt"),
            Path.Combine(pathToProject, $"r{secondRelease.ToString()}_misspellings_code.txt"),
            Path.Combine(pathToProject, $"r{secondRelease.ToString()}_unidentified_code.txt"),
            });
            var file2Lcomm = new List<string>(new string[] {
            Path.Combine(pathToProject, $"r{secondRelease.ToString()}_words_comments.txt"),
            Path.Combine(pathToProject, $"r{secondRelease.ToString()}_acronyms_comments.txt"),
            Path.Combine(pathToProject, $"r{secondRelease.ToString()}_misspellings_comments.txt"),
            Path.Combine(pathToProject, $"r{secondRelease.ToString()}_unidentified_comments.txt"),
            });


            // filling up two lists with words
            try
            {
                foreach (var f in file1Lcode)
                {
                    using (StreamReader file = new StreamReader(f))
                    {
                        while (!file.EndOfStream)
                        {
                            string line = file.ReadLine();
                            // the input in the form of "variableName 2432"
                            string[] splitLine = line.Split(null);
                            release1code.Add(splitLine[0]);
                        }
                    } 
                }
            }
            catch (Exception e)
            {
				Console.WriteLine($@"There was an error reading the file");
                Constants.log.Error(e.Message);
                throw;
            }
            try
            {
                foreach (var f in file1Lcomm)
                {
                    using (StreamReader file = new StreamReader(f))
                    {
                        while (!file.EndOfStream)
                        {
                            string line = file.ReadLine();
                            string[] splitLine = line.Split(null);
                            release1comm.Add(splitLine[0]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($@"There was an error reading the file");
                Constants.log.Error(e.Message);
                throw;
            }

            try
            {
                foreach (var f in file2Lcode)
                {
                    using (StreamReader file = new StreamReader(f))
                    {
                        while (!file.EndOfStream)
                        {
                            string line = file.ReadLine();
                            string[] splitLine = line.Split(null);
                            release2code.Add(splitLine[0]);
                        }
                    } 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($@"There was an error reading the file");
                Constants.log.Error(e.Message);
                throw;
            }
            try
            {
                foreach (var f in file2Lcomm)
                {
                    using (StreamReader file = new StreamReader(f))
                    {
                        while (!file.EndOfStream)
                        {
                            string line = file.ReadLine();
                            string[] splitLine = line.Split(null);
                            release2comm.Add(splitLine[0]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($@"There was an error reading the file");
                Constants.log.Error(e.Message);
                throw;
            }

            // finding intersection and union between them
            IEnumerable<string> intersectCode = release1code.Intersect(release2code);
			IEnumerable<string> unionCode = release1code.Union(release2code);

            IEnumerable<string> intersectComm = release1comm.Intersect(release2comm);
            IEnumerable<string> unionComm = release1comm.Union(release2comm);

            decimal codeChange = 1;
            decimal commChange = 1;

            try
            {
                codeChange = 1m - Math.Abs((decimal)intersectCode.Count()) / Math.Abs((decimal)unionCode.Count());
            }
            catch { }
            try
            {
                commChange = 1m - Math.Abs((decimal)intersectComm.Count()) / Math.Abs((decimal)unionComm.Count());
            }
            catch { }

            return (codeChange, commChange);           
        }

        /// <summary>
        /// Calculates linguistic change between 2 files
        /// </summary>
        /// <param name="pathToProject">Path to project</param>
        /// <param name="firstRelease">Path to txt with frequencies for 1st release</param>
        /// <param name="secondRelease">Path to txt with frequencies for 2nd release</param>
        /// <returns></returns>
        public decimal LinguisticChangeBetweenTwoReleases(string pathToProject, string firstRelease, string secondRelease)
        {
            List<string> release1Words = new List<string>();
            List<string> release2Words = new List<string>();

            if (!pathToProject.EndsWith(@"\"))
            {
                pathToProject = pathToProject + @"\";
            }

            // filling up two lists with words
            try
            {
                using (StreamReader file = new StreamReader(firstRelease))
                {
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        // the input in the form of "variableName 2432"
                        string[] splitLine = line.Split(null);
                        release1Words.Add(splitLine[0]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($@"There was an error reading the file: {firstRelease}");
                Constants.log.Error(e.Message);
                throw;
            }

            try
            {
                using (StreamReader file = new StreamReader(secondRelease))
                {
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        // the input in the form of "variableName 2432"
                        string[] splitLine = line.Split(null);
                        release2Words.Add(splitLine[0]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($@"There was an error reading the file: {secondRelease}");
                Constants.log.Error(e.Message);
                throw;
            }

            // finding intersection and union between them
            IEnumerable<string> intersect = release1Words.Intersect(release2Words);
            IEnumerable<string> union = release1Words.Union(release2Words);

            return 1m - Math.Abs((decimal)intersect.Count()) / Math.Abs((decimal)union.Count());

        }

        public decimal AverageLinguisticChangeBetweenTwoReleases(string pathToProject, List<string> statFiles, int firstRelease, int secondRelease)
        {
            List<decimal> ret = new List<Decimal>();
            for (int i = firstRelease; i < secondRelease; i++)
            {
                ret.Add(LinguisticChangeBetweenTwoReleases(pathToProject, statFiles[i], statFiles[i + 1]));
            }
            return ret.Average();
        }

        /// <summary>
        /// Calculates the linguistic change for all projects (code + comments) and writes the results into 2 datasets
        /// </summary>
        /// <param name="path">Path to projects</param>
        public void LinguisticChangeAllProjectsToFile(string path)
        {
            // preparation
            string dataSetName = "LinguisticChange.txt";

            // creating dataset file
            Message = $@"Creating the dataset {dataSetName}...";
            try
            {
                if (!File.Exists($@"{dataSetName}"))
                {
                    string firstRow = $"fullName,ChangeCode,ChangeComments{Environment.NewLine}";
                    File.WriteAllText($@"{dataSetName}", firstRow);
                }
                else
                {
                    Message = $"{dataSetName} already exists. Appending...";
                }
            }
            catch (Exception e)
            {
                throw;
            }

            string[] projects = HelperFunctions.FindProjects(path);
            foreach (var project in projects)
            {
                using (var dataSet = new StreamWriter($@"{dataSetName}", true))
                {
                    var releaseNums = Directory.GetDirectories(project + @"\src\")
                    .Where(x => (Directory.GetFiles(x).Count() != 0))
                    .Select(x => int.Parse(Regex.Match(new DirectoryInfo(x).Name, @"\d+").Value)).ToList();
                    // list to hold ling. change values with corresponding release numbers
                    var lingChangeWithReleases = new List<Tuple<int, (decimal, decimal)>>();
                    var lc = new LinguisticChangeCalculator();
                    var res = lc.LinguisticChangeBetweenTwoReleases(project, releaseNums.Min(), releaseNums.Max());

                    WriteLingStatsToFile(new DirectoryInfo(project).Name, res, dataSet);
                }
            }

            Message = $@"The results are saved in: {dataSetName}";

            void WriteLingStatsToFile(string project, (decimal, decimal) result, StreamWriter dataSet)
            {
                try
                {
                    dataSet.Write($@"{project.Split('/').Last().Replace("_", "/")},");
                    dataSet.Write($@"{result.Item1},");
                    dataSet.Write($@"{result.Item2}");
                    dataSet.Write(Environment.NewLine);
                    dataSet.Flush();
                }
                catch (Exception e)
                {
                    dataSet.Flush();
                    throw;
                }
            }
        }

        /// <summary>
        /// Writes the linguistic change for a single project to file, by release.
        /// </summary>
        /// <param name="project">Project</param>
        public void WriteSingleProjectToFileByRelease(string project)
		{
			var path = project.Replace("/", "_") + "_LingChange.txt";
			Console.WriteLine($@"{path}...");
            try
            {
                if (!File.Exists(path))
                {
                    string firstRow = $"releaseNum,LingChange{Environment.NewLine}";
                    File.WriteAllText(path, firstRow);
                }
                else
                {
					Console.WriteLine($@"{path} already exists. Appending...");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error has occured while creating or writing to a dataset file: {e.Message}");
                Constants.log.Error(e.Message);
                Environment.Exit(1);
            } 

			using (StreamWriter dataSet = new StreamWriter(path, true))
			{
				var releases = HelperFunctions.FindReleases(project);
				for (int i = 1; i <= releases.Length; i++)
				{
                    var m = Regex.Matches(releases[i], @"\d+");
                    var num = Int32.Parse(m[m.Count - 1].Value);

                    dataSet.Write($@"{i},");
                    if (i == 1)
					{
						dataSet.Write($@"1");
					}
					else
					{
						dataSet.Write(LinguisticChangeBetweenTwoReleases(project.Replace(@"/", "_"), i - 1, i));
					}
					dataSet.Write(Environment.NewLine);
					dataSet.Flush();
				}
			}
        }

        /// <summary>
        /// Calculates quartile linguistic change
        /// </summary>
        /// <param name="pathToProject">Path to project</param>
        /// <returns>List with 4 elements - ling. change - for each consequitve quartile</returns>
        public List<decimal> LinguisticChangeQuartileCode(string pathToProject)
        {
            // find how many releases are there for a project
            HelperFunctions.FindReleases(pathToProject, out List<string> statFiles, out int numOfReleases);
            // split releases into bins
            int binSize = SplitIntoBins(numOfReleases, 4);
            // calculate ling change between first and last release for each bin
            int first = 0, last = binSize-1;
            List<decimal> ret = new List<decimal>(); // 4 numbers
            for (int i = 1; i <= 4; i++)
            {
                ret.Add(LinguisticChangeBetweenTwoReleases(pathToProject, statFiles[first], statFiles[last]));
                first = last + 1;
                last = (i+1) * binSize-1;
            }
            return ret;
        }

        /// <summary>
        /// Calculates average quartile linguistic change
        /// </summary>
        /// <param name="pathToProject">Path to project</param>
        /// <returns>List with 4 elements - ling. change - for each consequitve quartile</returns>
        public List<decimal> AverageLinguisticChangeQuartileCode(string pathToProject)
        {
            // find how many releases are there for a project
            HelperFunctions.FindReleases(pathToProject, out List<string> statFiles, out int numOfReleases);
            // split releases into bins
            int binSize = SplitIntoBins(numOfReleases, 4);
            // calculate ling change between first and last release for each bin
            int first = 0, last = binSize - 1;
            List<decimal> ret = new List<decimal>(); // 4 numbers
            for (int i = 1; i <= 4; i++)
            {
                ret.Add(AverageLinguisticChangeBetweenTwoReleases(pathToProject, statFiles, first, last));
                first = last + 1;
                last = (i + 1) * binSize - 1;
            }
            return ret;
        }


        /// <summary>
        /// Calculates and writes interquartile linguistic change for selected projects
        /// </summary>
        /// <param name="path">Path to projects</param>
        /// <param name="minNumOfReleases">Min # of releases a project has to have (deafult = 40)</param>
        /// <param name="isAverage">Calculate the average between consequtive releases instead of first and last only</param>
        public void WriteLinguisticChangeQuartileCodeForEachProjectToFile(string path, int minNumOfReleases = 40, bool isAverage = false)
        {
            string fileName = "";
            if (!isAverage) fileName = "LingChangeQuartile.txt";
            else fileName = "AverageLingChangeQuartile.txt";

            // prepare dataset
            Message = $@"Creating the dataset {Constants.PROJECTS_PATH}\{fileName}...";
            try
            {
                if (!File.Exists($@"{Constants.PROJECTS_PATH}\{fileName}"))
                {
                    string firstRow = $"fullName,1stQuartile,2ndQuartile,3rdQuartile,4thQuartile{Environment.NewLine}";
                    File.WriteAllText($@"{Constants.PROJECTS_PATH}\Console", firstRow);
                }
                else
                {
                    Message = $"{fileName} already exists. Appending...";
                }
            }
            catch (Exception e)
            {
                throw;
            }

            string[] projects = HelperFunctions.FindProjects(path);
            Message = $@"Found {projects.Length} projects!";
            foreach (var project in projects)
            {
                Message = $@"Processing {project}...";
                // check if it should be processed
                HelperFunctions.FindReleases(project, out List<string> statFiles, out int numOfReleases);
                if (numOfReleases < minNumOfReleases)
                {
                    Message = $@"Project {project} has only {numOfReleases} releases! Skipping...";
                    continue;
                }

                using (var dataSet = new StreamWriter($@"{Constants.PROJECTS_PATH}\{fileName}", true))
                {
                    var toWrite = new List<decimal>();
                    if (!isAverage) toWrite = LinguisticChangeQuartileCode(project);
                    else toWrite = AverageLinguisticChangeQuartileCode(project);

                    try
                    {
                        dataSet.Write($@"{project.Split('\\').Last().Replace("_", "/")},");
                        dataSet.Write($@"{toWrite[0]},");
                        dataSet.Write($@"{toWrite[1]},");
                        dataSet.Write($@"{toWrite[2]},");
                        dataSet.Write($@"{toWrite[3]}");
                        dataSet.Write(Environment.NewLine);
                        dataSet.Flush();
                    }
                    catch (Exception)
                    {
                        dataSet.Flush();
                        throw;
                    }
                }
            }
            Message = $@"The results are saved in: {fileName}";
        }

        public void WriteSyntacticFitnessForEachProjectToFileWords(string pathToProjects)
        {
            // prepare dataset
            Message = $@"Creating the dataset {pathToProjects}\SyntacticFitness.txt...";
            try
            {
                if (!File.Exists($@"{pathToProjects}\SyntacticFitness.txt"))
                {
                    string firstRow = $"fullName,NaturalWords,Acronyms,Misspellings{Environment.NewLine}";
                    File.WriteAllText($@"{pathToProjects}\SyntacticFitness.txt", firstRow);
                }
                else
                {
                    Message = $"SyntacticFitness.txt already exists. Appending...";
                }
            }
            catch (Exception)
            {
                throw;
            }

            string[] projects = HelperFunctions.FindProjects(pathToProjects);
            Message = $@"Found {projects.Length} projects!";
            Message = "Processing...";
            object sync = new object();
            Parallel.ForEach(projects, project =>
            {
                var survivalRates = Task.Run(() =>CalculateSyntacticFitnessForProjectWordTypes(project)).Result;
                lock (sync)
                {
                    using (var dataSet = new StreamWriter($@"{pathToProjects}\SyntacticFitness.txt", true))
                    {
                        try
                        {
                            dataSet.Write($@"{project.Split('\\').Last().Replace("_", "/")},");
                            dataSet.Write($@"{survivalRates[0]},");
                            dataSet.Write($@"{survivalRates[1]},");
                            dataSet.Write($@"{survivalRates[2]},");
                            dataSet.Write(Environment.NewLine);
                            dataSet.Flush();
                        }
                        catch (Exception)
                        {
                            dataSet.Flush();
                            throw;
                        }
                    }
                }

            });
            Message = $@"The results are saved in: {pathToProjects}\SyntacticFitness.txt";
        }

        public void CalculateSyntacticFitnessForEachProject(string pathToProjects, bool IsWordType)
        {
            if (IsWordType) WriteSyntacticFitnessForEachProjectToFileWords(pathToProjects);
            else WriteSyntacticFitnessForEachProjectToFileLengths(pathToProjects);
        }

        public void WriteSyntacticFitnessForEachProjectToFileLengths(string pathToProjects)
        {
            // prepare dataset
            Message = $@"Creating the dataset {pathToProjects}\SyntacticFitnessLengths.txt...";
            try
            {
                if (!File.Exists($@"{pathToProjects}\SyntacticFitnessLengths.txt"))
                {
                    string firstRow = $"fullName,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20{Environment.NewLine}";
                    File.WriteAllText($@"{pathToProjects}\SyntacticFitnessLengths.txt", firstRow);
                }
                else
                {
                    Message = $"SyntacticFitnessLengths.txt already exists. Appending...";
                }
            }
            catch (Exception)
            {
                throw;
            }

            string[] projects = HelperFunctions.FindProjects(pathToProjects);
            Message = $@"Found {projects.Length} projects!";
            object sync = new object();
            Parallel.ForEach(projects, project =>
            {
                var survivalRates = Task.Run(() => CalculateSyntacticFitnessForProjectLength(project)).Result;
                lock (sync)
                {
                    using (var dataSet = new StreamWriter($@"{pathToProjects}\SyntacticFitnessLengths.txt", true))
                    {
                        try
                        {
                            dataSet.Write($@"{project.Split('\\').Last().Replace("_", "/")},");
                            foreach (var elem in survivalRates)
                            {
                                dataSet.Write($@"{elem.Value},");
                            }
                            dataSet.Write(Environment.NewLine);
                            dataSet.Flush();
                        }
                        catch (Exception)
                        {
                            dataSet.Flush();
                            throw;
                        }
                    }
                }          
            });
            Message = $@"The results are saved in: {pathToProjects}\SyntacticFitnessLengths.txt";
        }

        /// <summary>
        /// Calculates syntactic fitness for all words
        /// </summary>
        /// <param name="pathToProject">Path</param>
        /// <returns>Words with fitness values</returns>
        public List<string> CalculateSyntacticFitnessForProjectWordTypes(string pathToProject)
        {
            // extract a list of all unique words for each category
            var words = GetUniqueWords(pathToProject, "words_code");
            var acronyms = GetUniqueWords(pathToProject, "acronyms_code");
            var misspellings = GetUniqueWords(pathToProject, "misspellings_code");

            // get a List<Dictionary> for all tokens in project
            var listOfReleasesWithWords = new List<Dictionary<string, int>>();
            HelperFunctions.FindReleases(pathToProject, out int numOfReleases);
            for (int i = 1; i <= numOfReleases; i++)
            {
                listOfReleasesWithWords.Add(GetWordsForSpecificRelease(pathToProject, i));
            }

            // calculate the average word utility
            var wordsUtility = new List<double>();
            var acronymsUtility = new List<double>();
            var misspellingsUtility = new List<double>();

            foreach (var word in words)
            {
                wordsUtility.Add(AverageWordUtility(listOfReleasesWithWords, word));
            }
            foreach (var word in acronyms)
            {
                acronymsUtility.Add(AverageWordUtility(listOfReleasesWithWords, word));
            }
            foreach (var word in misspellings)
            {
                misspellingsUtility.Add(AverageWordUtility(listOfReleasesWithWords, word));
            }
            string wordsAvg, acronymsAvg, misspellingsAvg;
            try
            {
                wordsAvg = (wordsUtility.Average() * 100D).ToString();
            }
            catch
            {
                wordsAvg = "N/A";
            }
            try
            {
                acronymsAvg = (acronymsUtility.Average() * 100D).ToString();
            }
            catch
            {
                acronymsAvg = "N/A";
            }
            try
            {
                misspellingsAvg = (misspellingsUtility.Average() * 100D).ToString();
            }
            catch
            {
                misspellingsAvg = "N/A";
            }
            return new List<string>() { wordsAvg, acronymsAvg, misspellingsAvg };
        }

        public SortedDictionary<int, string> CalculateSyntacticFitnessForProjectLength(string pathToProject)
        {
            // extract a list of all unique words for each category
            var words = new HashSet<string>();
            words = GetUniqueWords(pathToProject, "words");
            words.UnionWith(GetUniqueWords(pathToProject, "acronyms"));
            words.UnionWith(GetUniqueWords(pathToProject, "misspellings"));

            // get a List<Dictionary> for all tokens in project
            var listOfReleasesWithWords = new List<Dictionary<string, int>>();
            HelperFunctions.FindReleases(pathToProject, out List<string> statFiles, out int numOfReleases);
            for (int i = 1; i <= numOfReleases; i++)
            {
                listOfReleasesWithWords.Add(GetWordsForSpecificRelease(pathToProject, i));
            }

            // calculate the average word utility
            // holds lengths and their correspondings utilities
            var lengthsDic = new Dictionary<int, List<double>>();
            // we only consider word lenghts 20 at most
            for (int i = 1; i <= 20; i++) lengthsDic.Add(i, new List<double>());

            foreach (var word in words)
            {
                if (word.Length <= 20)
                    lengthsDic[word.Length].Add(AverageWordUtility(listOfReleasesWithWords, word));
            }

            var ret = new SortedDictionary<int, string>();  
            foreach (var elem in lengthsDic)
            {
                string val;
                if (elem.Value.Count == 0) val = "N/A";
                else val = elem.Value.Average().ToString();
                ret.Add(elem.Key, val);
            }


            return ret;
        }

        public void WriteWordBirthAndDeathRatesForAProjectToFile(string pathToProject)
        {
            // prepare dataset
            Message = $@"Creating the dataset {pathToProject}\BirthDeathRates.txt...";

            Message = $@"Processing {pathToProject}...";
            using (var dataSet = new StreamWriter($@"{pathToProject}\BirthDeathRates.txt", true))
            {
                var birthsDeaths = CalculateWordBirthAndDeathForProject(pathToProject);

                try
                {
                    dataSet.Write("WordBirth,WordDeath");
                    dataSet.Write(Environment.NewLine);
                    for (int i = 0; i < birthsDeaths.Item1.Count; i++)
                    {
                        dataSet.Write($@"{birthsDeaths.Item1[i]},{birthsDeaths.Item2[i]}");
                        dataSet.Write(Environment.NewLine);
                    }       
                    dataSet.Flush();
                }
                catch (Exception)
                {
                    dataSet.Flush();
                    throw;
                }
            }

            Message = $@"The results are saved in: {pathToProject}\BirthDeathRates.txt";
        }

        /// <summary>
        /// Calculates word births and deaths rates for each conseq. release, for a project
        /// </summary>
        /// <param name="pathToProject"></param>
        /// <returns>A tuple of lists with birth rates and death rates</returns>
        private Tuple<List<double>, List<double>> CalculateWordBirthAndDeathForProject(string pathToProject)
        {
            // extract a list of all unique words for each category
            var words = GetUniqueWords(pathToProject, "words");
            var acronyms = GetUniqueWords(pathToProject, "acronyms");
            var misspellings = GetUniqueWords(pathToProject, "misspellings");

            // get a List<Dictionary> for all tokens in project
            var listOfReleasesWithWords = new List<Dictionary<string, int>>();
            var releases = HelperFunctions.FindReleases(pathToProject);

            var pat = @"release(\d+)";
            var listOfNums = new List<int>();
            foreach (var r in releases)
            {
                var m = Regex.Match(r, pat);
                var num = Int32.Parse(m.Groups[1].Value);
                listOfNums.Add(num);
            }

            listOfNums.Sort();

            for (int i = 0; i < listOfNums.Count; i++)
            {
                listOfReleasesWithWords.Add(GetWordsForSpecificRelease(pathToProject, listOfNums[i]));
            }

            // calculate birth and death rates for each consequtive release
            var birthRates = new List<double>();
            var deathRates = new List<double>();
            for (int i = 0; i < listOfReleasesWithWords.Count - 1; i++)
            {
                Message = $@"Calculating for release {i + 2}...";
                var ret = CalculateWordBirthAndDeathBetweenTwoReleases(listOfReleasesWithWords[i], listOfReleasesWithWords[i + 1]);
                birthRates.Add(ret.Item1);
                deathRates.Add(ret.Item2);
            }
            return new Tuple<List<double>, List<double>>(birthRates, deathRates);
        }

        /// <summary>
        /// Calculates word birth and word death between 2 releases by formula: new (dead) words in release 2 / total words in release 2
        /// </summary>
        /// <param name="release1"></param>
        /// <param name="release2"></param>
        /// <returns>A tuple (new words, dead words)</returns>
        private Tuple<double, double> CalculateWordBirthAndDeathBetweenTwoReleases(Dictionary<string, int> release1, Dictionary<string, int> release2)
        {
            //double totalWords = (from elem in release2
            //                     select elem.Value).Sum();
            double totalWords = release2.Count();
            //double wordBirth = release2.Keys.Except(release1.Keys).Select(x => release2[x]).Sum();
            //double wordDeath = release1.Keys.Except(release2.Keys).Select(x => release1[x]).Sum();
            double wordBirth = release2.Keys.Except(release1.Keys).Count();
            double wordDeath = release1.Keys.Except(release2.Keys).Count();
            return new Tuple<double, double>(wordBirth / totalWords, wordDeath / totalWords);
        }

        private HashSet<string> GetUniqueWords(string pathToProject, string fileName)
        {
            var temp = Directory.GetFiles(pathToProject, @"*.txt");
            var listOfTxt = (from txt in temp
                             where Regex.IsMatch(Path.GetFileName(txt), $@"r\d+_{fileName}.txt")
                             select txt).ToList();
            var words = new HashSet<string>();
            foreach (var txt in listOfTxt)
            {
                string line;
                using (var reader = new StreamReader(txt))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        words.Add(line.Split()[0]);
                    }
                }
            }
            return words;
        }

        private Dictionary<string, int> GetWordsForSpecificRelease(string pathToProject, int releaseNum)
        {
            var temp = Directory.GetFiles(pathToProject, @"*.txt");
            var listOfTxt = (from txt in temp
                             where Regex.IsMatch(Path.GetFileName(txt), $@"r{releaseNum}_(words|acronyms|misspellings)_(code|comments)\.txt")
                             select txt).ToList();
            var words = new Dictionary<string, int>();
            foreach (var txt in listOfTxt)
            {
                string line;
                using (var reader = new StreamReader(txt))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        words.TryAdd(line.Split()[0], Convert.ToInt32(line.Split()[1]));
                    }
                }
            }
            return words;
        }

        private int SplitIntoBins(int _numOfReleases, int _numOfBins)
        {
            return _numOfReleases / _numOfBins;
        }

    }
}
