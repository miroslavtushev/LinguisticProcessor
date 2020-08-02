using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace SEEL.LinguisticProcessor
{
	/// <summary>
    /// This class downloads the projects for a given list of links
    /// </summary>
    class ProjectDownloader : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
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
		private readonly string _linksFile;
		private const string ZIP_LOCATION = "zips";
		private const string SRC_FILES_LOCATION = "src";
        /// <summary>
        /// Programming language used
        /// </summary>
        private string m_targetLang;
        public ProjectDownloader(string linksFile, ProgrammingLanguage progLang)
		{
			_linksFile = linksFile;
            if (progLang == ProgrammingLanguage.Java)
            {
                m_targetLang = Constants.JAVA_LANG;
            }
            else if (progLang == ProgrammingLanguage.CSharp)
            {
                m_targetLang = Constants.CSHARP_LANG;
            }
            else if (progLang == ProgrammingLanguage.Python)
            {
                m_targetLang = Constants.PYTHON_LANG;
            }
            else if (progLang == ProgrammingLanguage.JavaScript)
            {
                m_targetLang = Constants.JAVASCRIPT_LANG;
            }
            else if (progLang == ProgrammingLanguage.Go)
            {
                m_targetLang = Constants.GO_LANG;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private List<(string, string)> RetrieveReleasesUrls(string line)
        {
            Message = $"Processing: {line}";
            Util.HelperFunctions.ExtractUserAndRepoFromLink(line, out string username, out string repo);

            IReadOnlyList<Octokit.RepositoryTag> releases = null;
            try
            {
                releases = Task.Run(() => Authenticator.client.Repository.GetAllTags(username, repo)).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception has occured: {e}. Perhaps, the link you've provided is broken or in the wrong format");
                Constants.log.Error(e.Message);
                Environment.Exit(1);
            }

            // Item1 = URL, Item2 = Name
            var urls = new List<(string, string)>();
            foreach (var elem in releases)
            {
                urls.Add((elem.ZipballUrl, elem.Name));
            }
            return urls;
        }

        /// <summary>
        /// Downloads the releases into ZIP folder
        /// </summary>
        /// <param name="urls"><list type=" of urls of the releases"</param>
        /// <param name="user">Username of the project</param>
        /// <param name="repo">Repository name of the project</param>
        private async Task DownloadReleases(List<(string, string)> urls, string user, string repo)
        {
            // creating directory to store project's releases
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), user + "_" + repo);

            // if some releases have been downloaded
            if (Directory.Exists(path))
            {
                Message = "Some releases have been downloaded already. Checking...";
                // check what's left to be downloaded
                // get all downloaded releases so far
                var downloadedReleases = Directory.GetFiles(Path.Combine(path, ZIP_LOCATION));
                // if nothing is there, which means everything has been downloaded - return
                if (downloadedReleases.Length == 0)
                {
                    Message = "The releases have been downloaded.";
                    return;
                }
                // to store the list of release numbers
                List<int> listOfReleaseNumbers = new List<int>();

                // extract the numbers and put them into a list
                foreach (var str in downloadedReleases)
                {
                    var num = Regex.Match(str, @"\d+").Value;
                    listOfReleaseNumbers.Add(int.Parse(num));
                }

                // find the minimum one
                var minRelease = listOfReleaseNumbers.Min();
                // if we have all releases - skip
                if (minRelease == 1) 
                    return;

                // e.g. if there are 64 releases and minRelease==32, we need to download the remaining releases from 32 to 1.
                // Keep in mind that the last downloaded release is corrupted, so we need to remove it and redownload
                urls.RemoveRange(0, listOfReleaseNumbers.Count - 1);
                await _DownloadNumberOfReleases(urls.Select(x => x.Item1).ToList(), urls.Count);
                return;
            }
            else if (urls.Count > 1) // if there are at least 2 releases
            {
                Directory.CreateDirectory(path);
                // create path for code and zip files as well
                Directory.CreateDirectory(Path.Combine(path, ZIP_LOCATION));
                Directory.CreateDirectory(Path.Combine(path, SRC_FILES_LOCATION));

                // storing names of releases for future reference
                File.WriteAllLines(Path.Combine(path, "names.txt"), urls.Select(x => x.Item2).Reverse());
               
            }
            // else proceed to the next repo (to avoid creating empty folders)
            else
			{
                Message = $"Too few or no releases are available for {user}/{repo}";
				return;
			}

            // downloading all releases
            await _DownloadNumberOfReleases(urls.Select(x => x.Item1).ToList(), urls.Count);
            async Task _DownloadNumberOfReleases(List<string> _urls, int _minRelease)
            {
                // remove the last downloaded release
                File.Delete(Path.Combine(path, ZIP_LOCATION, $"release{(_minRelease).ToString()}Z.zip"));
                //_minRelease = _minRelease + 1;
                for(int i = 0; i < _minRelease; i++)
                {
                    MyWebClient myWebClient = new MyWebClient(600000);
                    myWebClient.Proxy = null;
                    myWebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.33 Safari/537.36");

                    // format: "release36Z.zip", "release35Z.zip", etc....
                    var filename = $"release{(_minRelease - i).ToString()}Z.zip";

                    Message = $"Downloading {user}/{repo}: {i + 1}/{_minRelease}";
                    try
                    {
                        await myWebClient.DownloadFileTaskAsync(new Uri(_urls[i]), Path.Combine(path, ZIP_LOCATION, filename));
                    }
                    catch (WebException)
                    {
                        Message = "File is not found on the server. Skipping...";
                        return;
                    }
                }
                await Task.Run(() => GetSourceFilesFromZipballs(path));
            }
        }

        void GetSourceFilesFromZipballs(string path)
        {
            ZipArchive OpenRead(string filename)
            {
                return new ZipArchive(File.OpenRead(filename), ZipArchiveMode.Read);
            }

            string[] zipFiles = Directory.GetFiles(Path.Combine(path, ZIP_LOCATION), "release*.zip", SearchOption.TopDirectoryOnly);

            if (zipFiles.Length != 0)
            {
                Message = $"Found {zipFiles.Length} zipfiles";
            }
            else
            {
                Message = "No zip files have been found";
                return;
            }
            Parallel.ForEach(zipFiles, zipF =>
            {
            Message = $"Extracting {zipF}...";
               try
               {
                   Message = $@"Creating {zipF.Substring(0, zipF.IndexOf(@"Z.", StringComparison.Ordinal))}...";
                   string temp = new DirectoryInfo(zipF.Substring(0, zipF.IndexOf(@"Z.", StringComparison.Ordinal))).Name;
                   string createdSrcDir = Path.Combine(path, SRC_FILES_LOCATION, temp);
                   Directory.CreateDirectory(createdSrcDir);

                   using (ZipArchive zip = OpenRead(zipF))
                   {
                       foreach (ZipArchiveEntry entry in zip.Entries)
                       {
                           if (entry.FullName.EndsWith(m_targetLang, StringComparison.OrdinalIgnoreCase))
                           {
                               try
                               {
                                   Message = $@"Found {entry.Name}";
                                   entry.ExtractToFile(Path.Combine(createdSrcDir, entry.Name), false);
                               }
                               catch (IOException)
                               {
                                   Message = $@"File {entry.Name} already exists! Renaming...";
                                   CreateFileName(entry, createdSrcDir);
                               }
                           }
                       }
                   }
               }
               catch (Exception e)
               {
                   Console.WriteLine(e);
                   Constants.log.Error(e.Message);
                   return;
               }
            });
            Message = "Cleaning up...";
            try
            {
                foreach (var zipF in zipFiles)
                {
                    Message = $"Removing: {zipF}";
                    File.Delete(zipF);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Constants.log.Error(e.Message);
                return;
            }
        }

        /// <summary>
        /// Renames the file with conflicting name
        /// </summary>
        /// <param name="fileName">The original file name</param>
        /// <param name="javaFile">Path to the file</param>
        /// <param name="path">Path to the projects</param>
        /// <param name="releaseDir">Current release directory</param>
        private void CreateFileName(ZipArchiveEntry entry, string path)
		{
			string newFileName = "";
            int digitsToRemove = 0;
            if (m_targetLang == Constants.JAVA_LANG)
            {
                digitsToRemove = 5;
            }
            else if (m_targetLang == Constants.CSHARP_LANG)
            {
                digitsToRemove = 3;
            }
            else if (m_targetLang == Constants.PYTHON_LANG)
            {
                digitsToRemove = 3;
            }
            else if (m_targetLang == Constants.JAVASCRIPT_LANG)
            {
                digitsToRemove = 3;
            }
            else if (m_targetLang == Constants.GO_LANG)
            {
                digitsToRemove = 3;
            }
            else
            {
                throw new NotImplementedException();
            }

            // if "filename_1.java"
            Regex rgx = new Regex($@"_(?<number>\d+)\{m_targetLang}");
            if (rgx.IsMatch(entry.Name))
            {
                // rename the file with the next digit                           
                Match match = rgx.Match(entry.Name);
                var num = int.Parse(match.Groups["number"].Value);
                newFileName = rgx.Replace(entry.Name, (num + 1).ToString());
            }
            // else filename.java
            else
            {
				// try this name and check if this file already exists

				int fileNumber = 1;
				newFileName = entry.Name.Remove(entry.Name.Length - digitsToRemove) + "_" + fileNumber.ToString() + m_targetLang;            
				while (File.Exists(Path.Combine(path, newFileName)))
				{
                    Message = $"File {newFileName} already exists!";
					fileNumber += 1;
					newFileName = entry.Name.Remove(entry.Name.Length - digitsToRemove) + "_" + fileNumber.ToString() + m_targetLang;
				}
            }
            Message = $"Renaming {Path.GetFileName(entry.Name)} -> {newFileName}";
            try
            {
                entry.ExtractToFile(Path.Combine(path, newFileName), false);
            }
            catch (Exception e)
            {
                Util.HelperFunctions.ShowMessageBox(e.Message);
                return;
            }
		}

        /// <summary>
        /// Runs the program
        /// </summary>
        /// <returns>void</returns>
        public async Task Run()
		{
			Authenticator.Authenticate();
            string[] reposToDownload = File.ReadAllLines(_linksFile);
            foreach (string line in reposToDownload)
            {
                var urls = RetrieveReleasesUrls(line);
                Util.HelperFunctions.ExtractUserAndRepoFromLink(line, out string username, out string repo);
                await DownloadReleases(urls, username, repo);
            }
		}
    }
}

