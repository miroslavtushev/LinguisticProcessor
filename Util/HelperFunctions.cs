using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SEEL.LinguisticProcessor.Exceptions;

namespace SEEL.LinguisticProcessor.Util
{
    public class HelperFunctions
    {
        public event EventHandler WriteLineCalled = delegate { };

        #region WriteLine
        public void WriteLine(string message)
		{
			Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] ");
			Console.Write(message);
			Console.Write(Environment.NewLine);
            this.WriteLineCalled(this, new EventArgs());
        }

        public  void WriteLine(int message)
		{
			Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] ");
			Console.Write(message);
			Console.Write(Environment.NewLine);
        }

		public  void WriteLine(decimal message)
		{
			Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] ");
			Console.Write(message);
			Console.Write(Environment.NewLine);
        }

		public  void WriteLine(Exception message)
		{
			Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] ");
			Console.Write(message);
			Console.Write(Environment.NewLine);
        }
        #endregion

        /// <summary>
        /// Finds the paths to txt release files with extracted words and counts them
        /// </summary>
        /// <param name="pathToProject">Path to a project</param>
        /// <param name="statFiles">Ordered list of paths to all releases</param>
        /// <param name="numOfReleases">Number of all releases</param>
        /// <param name="mode">Mode. false for code, true for comments</param>
        public static void FindReleases(string pathToProject, out List<string> statFiles, out int numOfReleases)
		{
            // NOT WORKING
            var reg = new Regex($@"\d+\.txt");
            statFiles = Directory.GetFiles(pathToProject, "*.txt", SearchOption.TopDirectoryOnly)
                .Where(x => reg.IsMatch(x))
                .OrderBy(x => int.Parse(Regex.Match(x, @"(\d+)\.txt").Groups[1].Value))
                .ToList();
            numOfReleases = statFiles.Count;
        }

        public static void FindReleases(string pathToProject, out int numOfReleases)
        {
            numOfReleases = Directory.GetDirectories(Path.Combine(pathToProject, "src"))
                .Select(x => new DirectoryInfo(x).Name)
                .Max(x => int.Parse(Regex.Match(x, @"release(\d+)").Groups[1].Value));
        }

        /// <summary>
        /// Finds the releases for a project
        /// </summary>
        /// <returns>The list of releases' paths</returns>
        /// <param name="pathToProject">Path to a project</param>
		public static string[] FindReleases(string pathToProject)
        {
            if (!Directory.Exists($@"{pathToProject}\src"))
            {
                Console.WriteLine($@"An error occured while reading the project: {pathToProject}. No releases were found.");
                throw new NoReleasesFoundException();
            }
            string[] releases = Directory.GetDirectories($@"{pathToProject}\src");
            Console.WriteLine($@"Found {releases.Length} releases in {pathToProject}");
            return releases;
        }
        
        /// <summary>
        /// Finds the projects
        /// </summary>
        /// <returns>The list of projects</returns>
        /// <param name="pathToProjects">Path to projects' location on a hard drive</param>
		public static string[] FindProjects(string pathToProjects)
        {
            if (!Directory.Exists($@"{pathToProjects}"))
            {
                Console.WriteLine($@"An error occured while reading the project: {pathToProjects}. No projects were found.");
                Environment.Exit(1);
            }
            string[] projects = Directory.GetDirectories($@"{pathToProjects}");
            Console.WriteLine($@"Found {projects.Length} projects!");
            return projects;
        }

        /// <summary>
        /// Finds the archived projects are extracts them to a temp folder specified in Constants
        /// </summary>
        /// <param name="pathToArchivedProjects">Path to archived projects' location</param>
        /// <returns>The list of unarchived projects</returns>
        public static string[] FindAndUnarchiveProjects(string pathToArchivedProjects)
        {
            if (!Directory.Exists($@"{pathToArchivedProjects}"))
            {
                Console.WriteLine($@"An error occured while reading the project: {pathToArchivedProjects}. No projects were found.");
                Environment.Exit(1);
            }
            string[] projects = Directory.GetFiles($@"{pathToArchivedProjects}");
            Console.WriteLine($@"Found {projects.Length} projects!");

            //create temporary directory to store unarchived projects
            var tempDir = Constants.PROJECTS_PATH + new DirectoryInfo(pathToArchivedProjects).Name;
            Directory.CreateDirectory(tempDir);

            Console.WriteLine($@"Unarchiving to {tempDir}");
            foreach(var project in projects)
            {
                // create a dir with archive's name
                var extractTo = tempDir + @"\" + Path.GetFileName(project).Replace(".zip", string.Empty);
                Directory.CreateDirectory(extractTo);
                Console.WriteLine($@"Extracting to {extractTo}...");
                ZipFile.ExtractToDirectory(project, extractTo);
            }
            string[] extractedProjects = Directory.GetDirectories(tempDir);
            return projects;
        }

        /// <summary>
        /// Extracts the username and repo given a link to a GH project
        /// </summary>
        /// <param name="userAndRepo">The link to a GH project</param>
        /// <param name="user">Username</param>
        /// <param name="repo">Repository</param>
        public static void ExtractUserAndRepoFromLink(string userAndRepo, out string user, out string repo)
		{
			string[] usernameAndRepo;
			user = repo = "";
            try
            {
				usernameAndRepo = userAndRepo.Split(new char[] { '/' });
                user = usernameAndRepo[usernameAndRepo.Length - 2];
                repo = usernameAndRepo[usernameAndRepo.Length - 1];
            }
            catch (Exception e)
            {
				Console.WriteLine($"An exception has occured when parsing the github link: {userAndRepo}: {e}");
                Constants.log.Error(e.Message);
                Environment.Exit(1);
            }
		}

        /// <summary>
        /// Extracts the username and repo given a link to a GH project
        /// </summary>
        /// <param name="userAndRepo">The path to a GH project</param>
        /// <param name="user">Username</param>
        /// <param name="repo">Repository</param>
        public static void ExtractUserAndRepoFromPath(string userAndRepo, out string user, out string repo)
        {
            string[] usernameAndRepo;
            user = repo = "";
            try
            {
                var tmp = new DirectoryInfo(userAndRepo).Name;
                // return 2 substrings to handle cases like "matterport_Mask_RCNN"
                usernameAndRepo = tmp.Split(new char[] { '_' }, 2);
                user = usernameAndRepo[usernameAndRepo.Length - 2];
                repo = usernameAndRepo[usernameAndRepo.Length - 1];
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception has occured when parsing the github link: {userAndRepo}: {e}");
                Constants.log.Error(e.Message);
                Environment.Exit(1);
            }
        }

        public static void ProjectsExtractorToSDD()
        {
            foreach (var path in new List<string> { Constants.JAVA_PROJECTS_PATH, Constants.CSHARP_PROJECTS_PATH, Constants.PYTHON_PROJECTS_PATH, Constants.JAVASCRIPT_PROJECTS_PATH })
            {
                Console.WriteLine($@"Processing {path}...");
                // locate the projects
                string[] compressedProjects = Directory.GetFiles(path);
                Console.WriteLine($@"Found {compressedProjects.Length} projects!");
                // create a folder on an SSD
                var langPath = Constants.PROJECTS_PATH + new DirectoryInfo(path).Name;
                Directory.CreateDirectory(langPath);
                foreach (var compressedProject in compressedProjects)
                {
                    Console.WriteLine($@"Processing {compressedProject}...");
                    // check if it has been processed already
                    var extractTo = langPath + @"\" + new DirectoryInfo(compressedProject).Name.Split(new string[] { ".zip" }, StringSplitOptions.None)[0];
                    if (Directory.Exists(extractTo)) continue;
                    // might be something else
                    if (!compressedProject.EndsWith(".zip")) continue;
                    Directory.CreateDirectory(extractTo);
                    // extract the contents
                    ZipFile.ExtractToDirectory(compressedProject, extractTo);
                }
            }
        }
        /// <summary>
        /// Used to identify which Names Extractor to use (C#, Java, etc.)
        /// </summary>
        /// <param name="pathToProject">Path to project</param>
        /// <returns>Instance of Names Extractor, programming language extension</returns>
        public static (NamesExtractors.BaseNamesExtractor, string) IdentifyNamesExtractor(string pathToProject)
        {
            // check what kind of files we are dealing with
            string[] files = null;
            try
            {
                files = Directory.GetFiles(pathToProject + @"\src", "*", SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            // I don't know how to make it better...
            var extDic = new Dictionary<string, int>();
            foreach (var f in files)
            {
                var e = Path.GetExtension(f);
                if (extDic.ContainsKey(e))
                {
                    extDic[e] += 1;
                }
                else extDic.Add(e, 1);
            }
            var programmingLanguage = extDic.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            switch (programmingLanguage)
            {
                case ".cs":
                    return (new NamesExtractors.CSharpNamesExtractor(pathToProject), programmingLanguage);
                case ".java":
                    return (new NamesExtractors.JavaNamesExtractor(pathToProject), programmingLanguage);
                case ".py":
                    return (new NamesExtractors.PythonNamesExtractor(pathToProject), programmingLanguage);
                case ".js":
                    return (new NamesExtractors.JavascriptNamesExtractor(pathToProject), programmingLanguage);
                case ".go":
                    return (new NamesExtractors.GoNamesExtractor(pathToProject), programmingLanguage);
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Displays a window with error message
        /// </summary>
        /// <param name="message">Error message</param>
        public static void ShowMessageBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);
        }
        /// <summary>
        /// Displays a window when a user changes windows
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Used to make sure that a user doesn't close the window with results accidentally
        /// </remarks>
        public static DialogResult ShowChangeWindowBox()
        {
            return MessageBox.Show("The current window will be closed. Do you wish to continue?", "Information",
                MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);
        }
       
    }
}
