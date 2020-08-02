using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// This class collect descriptive statistics on the dataset, such as: KLOC, avg. # of releases, etc.
    /// </summary>
    public class DescriptiveStatistics : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged implementation

        /// <summary>
        /// Path to directory with projects
        /// </summary>
        string m_projectsDir;

        /// <summary>
        /// For a single project
        /// </summary>
        string m_projectPath;

        /// <summary>
        /// Target langauge to compute statistics for
        /// </summary>
        string m_targetLang;

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

        public DescriptiveStatistics (ProgrammingLanguage lang)
        {
            if (lang == ProgrammingLanguage.Java)
            {
                m_projectsDir = Constants.JAVA_PROJECTS_PATH;
                m_targetLang = Constants.JAVA_LANG;
            }
            else if (lang == ProgrammingLanguage.CSharp)
            {
                m_projectsDir = Constants.CSHARP_PROJECTS_PATH;
                m_targetLang = Constants.CSHARP_LANG;
            }
            else if (lang == ProgrammingLanguage.Python)
            {
                m_projectsDir = Constants.PYTHON_PROJECTS_PATH;
                m_targetLang = Constants.PYTHON_LANG;
            }
            else if (lang == ProgrammingLanguage.JavaScript)
            {
                m_projectsDir = Constants.JAVASCRIPT_PROJECTS_PATH;
                m_targetLang = Constants.JAVASCRIPT_LANG;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        //public DescriptiveStatistics(ProgrammingLanguage lang, string path)
        //{
        //    switch (lang)
        //    {
        //        case ProgrammingLanguage.Java:
        //            m_targetLang = Constants.JAVA_LANG;
        //            break;
        //        case ProgrammingLanguage.CSharp:
        //            m_targetLang = Constants.CSHARP_LANG;
        //            break;
        //        case ProgrammingLanguage.Python:
        //            m_targetLang = Constants.PYTHON_LANG;
        //            break;
        //        case ProgrammingLanguage.JavaScript:
        //            m_targetLang = Constants.JAVASCRIPT_LANG;
        //            break;
        //        default:
        //            break;
        //    }
        //    m_projectPath = path;
        //}

        public static DescriptiveStatistics WithSingleProject(ProgrammingLanguage lang, string path)
        {
            var d = new DescriptiveStatistics(lang);
            d.m_projectPath = path;
            return d;
        }

        public static DescriptiveStatistics WithProjectsPath(ProgrammingLanguage lang, string path)
        {
            var d = new DescriptiveStatistics(lang);
            d.m_projectsDir = path;
            return d;
        }

        public DescriptiveStatistics(string lang, string path)
        {
            switch (lang)
            {
                case ".java":
                    m_targetLang = Constants.JAVA_LANG;
                    m_projectsDir = path;
                    break;
                case ".cs":
                    m_targetLang = Constants.CSHARP_LANG;
                    m_projectsDir = path;
                    break;
                case ".py":
                    m_targetLang = Constants.PYTHON_LANG;
                    m_projectsDir = path;
                    break;
                case ".js":
                    m_targetLang = Constants.JAVASCRIPT_LANG;
                    m_projectsDir = path;
                    break;
                default:
                    break;
            }
            m_projectPath = path;
        }

        /// <summary>
        /// Counts the number of source code files for a project
        /// </summary>
        /// <returns></returns>
        public int CountNumberOfFiles()
        {
            return Directory.GetFiles(m_projectPath, "*" + m_targetLang, SearchOption.AllDirectories).Length;
        }

        /// <summary>
        /// Counts the KLOC for a project
        /// </summary>
        /// <returns></returns>
        public ulong CountKLOCForProject()
        {
            ulong totalKLOC = 0;
            foreach (var file in Directory.GetFiles(m_projectPath, "*" + m_targetLang, SearchOption.AllDirectories))
            {
                using (StreamReader stream = new StreamReader(file))
                {
                    int i = 0;
                    while (stream.ReadLine() != null) { i++; }
                    totalKLOC += (ulong)i;
                } 
            }
            return totalKLOC;
        }

        /// <summary>
        /// Counts the total # of releases and the average # of releases per project
        /// </summary>
        /// <returns>{total releases, average releases}</returns>
        private string[] CountReleases()
        {
            var projects = new List<string>(Directory.GetDirectories(m_projectsDir));
            // total # of releases for all projects
            int totalReleases = 0;
            Message = "Processing...";

            foreach (var project in projects)
            {
                Message = $@"Processing {project}...";
                // # of releases for this particular project
                var allFolders = new List<string>();
                var releases = new List<string>(Directory.GetDirectories(project + @"\src\"));
                if (releases.Count <= 2)
                    continue;

                foreach (var release in releases)
                {
                    if (Directory.GetFiles(release).Length != 0)
                    {
                        allFolders.Add(release);
                    }    
                }
                totalReleases += allFolders.Count;
            }
            float avgReleases = (float)totalReleases / (float)projects.Count;
            return new string[] { totalReleases.ToString(), avgReleases.ToString() };
        }

        /// <summary>
        /// Counts total KLOC for all projects and average KLOC for a project
        /// </summary>
        /// <returns>{total KLOC, average KLOC}</returns>
        /// <remarks>All credits of this algorithm go to: <see cref="http://www.nimaara.com/2018/03/20/counting-lines-of-a-text-file/"/></remarks>
        public string[] CountKLOC()
        {
            var projects = new List<string>(Directory.GetDirectories(m_projectsDir));
            // total # of KLOC for all projects
            ulong totalKLOC = 0;
            Message = "Processing...";

            foreach (var project in projects)
            {
                Message = $@"Processing {project}...";
                // # of KLOC for a project
                ulong projectKLOC = 0;
                // release paths of a project
                var releases = new List<string>(Directory.GetDirectories(project + @"\src\"));
                if (releases.Count <= 2)
                    continue;

                const char CR = '\r';
                const char LF = '\n';
                const char NULL = (char)0;
                char[] byteBuffer = new char[1024 * 1024];
                char detectedEOL = NULL;
                char currentChar = NULL;

                foreach (var release in releases)
                {
                    Message = $"Processing {release}...";
                    var files = Directory.GetFiles(release, "*" + m_targetLang);
                    foreach (var file in files)
                    {
                        using (StreamReader stream = new StreamReader(file))
                        {
                            int bytesRead;
                            while ((bytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                            {
                                for (var i = 0; i < bytesRead; i++)
                                {
                                    currentChar = byteBuffer[i];

                                    if (detectedEOL != NULL)
                                    {
                                        if (currentChar == detectedEOL)
                                        {
                                            projectKLOC++;
                                        }
                                    }
                                    else if (currentChar == LF || currentChar == CR)
                                    {
                                        detectedEOL = currentChar;
                                        projectKLOC++;
                                    }
                                }
                            }

                            if (currentChar != LF && currentChar != CR && currentChar != NULL)
                            {
                                projectKLOC++;
                            }
                            //    int i = 0;
                            //    while (stream.ReadLine() != null) { i++; }
                            //    projectKLOC += (ulong)i;
                        }
                    }
                }
                totalKLOC += projectKLOC;
            }
            ulong avgReleases = totalKLOC / (ulong)projects.Count;
            return new string[] { totalKLOC.ToString(), avgReleases.ToString() };
        }

        /// <summary>
        /// Counts the unique number of words in the release: source code, processed source code, and comments
        /// </summary>
        /// <param name="release"></param>
        /// <returns></returns>
        public List<int> CountWordsForRelease(string release)
        {
            var ret = new List<int>();
            // get the release #
            var releaseNum = Regex.Match(release, @"\d+").Groups[0].Value;
            // find appropriate files and fill out the list
            ret.Add(File.ReadLines($@"{m_projectPath}\code{releaseNum}.txt").Count());
            ret.Add(File.ReadLines($@"{m_projectPath}\comments{releaseNum}.txt").Count());
            return ret;
        }

        /// <summary>
        /// Runs the program
        /// </summary>
        public string Run()
        {
            string[] kloc = CountKLOC();
            string[] releases = CountReleases();

            string pathToFile = m_projectsDir + @"\DescriptiveStats.txt";
            using (StreamWriter st = new StreamWriter(pathToFile, true))
            {
                st.Write("Project: " + m_targetLang);
                st.Write(Environment.NewLine);
                st.Write($@"Total KLOC for all projects: {kloc[0]}");
                st.Write(Environment.NewLine);
                st.Write($@"Average KLOC for all projects: {kloc[1]}");
                st.Write(Environment.NewLine);
                st.Write($@"Total number of releases for all projects: {releases[0]}");
                st.Write(Environment.NewLine);
                st.Write($@"Average number of releases for all projects: {releases[1]}");
                st.Write(Environment.NewLine);
                st.Write(Environment.NewLine);
            }
            return pathToFile;
        }
    }
}
