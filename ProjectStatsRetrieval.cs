using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Octokit;

namespace SEEL.LinguisticProcessor
{
	/// <summary>
    /// This class retrives various stats for a list of projects, such as stars, main language, forks, etc.
    /// </summary>
	public class ProjectStatsRetrieval : INotifyPropertyChanged
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

        public bool OverwriteDataset { get; set; }

        /// <summary>
        /// The list of projects to retrieve the stats for
        /// </summary>
        private List<string> m_listOfProjects = new List<string>();

        private List<List<string>> m_listOfProjects2 = new List<List<string>>();

        private void Prepare()
        {
            Message = "Obtaining the projects...";//////////////////////////////////////////////
            foreach (var path in new List<string> { Constants.PYTHON_PROJECTS_PATH, Constants.JAVASCRIPT_PROJECTS_PATH })
            {
                m_listOfProjects.AddRange(Directory.GetDirectories(path));
            }

            if (!OverwriteDataset)
            {
                List<string> unprocessed = new List<string>();
                if (File.Exists("Project_stats.txt"))
                {
                    try
                    {
                        var temp = File.ReadAllLines("Project_stats.txt");
                        unprocessed = new List<string>(temp);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An error occured while reading dataset: {e}");
                        throw;
                    }
                    List<string> _temp = new List<string>();
                    foreach (var line in m_listOfProjects)
                    {
                        foreach (var elem in unprocessed)
                        {
                            if (elem.Contains(new DirectoryInfo(line).Name.Replace('_', '/')))
                            {
                                _temp.Add(line);
                                break;
                            }
                        }
                    }
                    m_listOfProjects = m_listOfProjects.Except(_temp).ToList();
                }
            }
            // easier to collect the # of releases at this step
            // { user, repo, releases }
            // don't need for now
                //var query = from p in listOfProjects
                //            let releases = Directory.GetDirectories(p + @"\src\").Where(x => (Directory.GetFiles(x).Count() != 0))
                //            let c = new DirectoryInfo(p).Name
                //            let d = c.IndexOf('_')
                //            select new List<string>
                //        {
                //            c.Substring(0, d),
                //            c.Substring(d + 1),
                //            releases.Count().ToString()
                //        };
                //// list of lists
                //Message = "Creating a list...";
                //foreach (var item in query)
                //{
                //    try
                //    {
                //        m_listOfProjects2.Add(item);
                //    }
                //    catch (ArgumentNullException)
                //    {
                //        continue;
                //    }
                //}     
        }

        /// <summary>
        /// Creates the dataset file to write to
        /// </summary>
        /// <exception cref="Exception">Thrown by IO</exception>
        public void CreateDataset()
        {
            Message = $"Creating the dataset Project_stats.txt...";
            try
            {
                if (!File.Exists($"Project_stats.txt"))
                {
                    string firstRow = $"id,name,fullName,url,stargazers,watchers,contributors,releases,commits,created,forksCount,language,hasDownloads,hasIssues,hasPages,hasWiki,size {Environment.NewLine}";
                    File.WriteAllText($"Project_stats.txt", firstRow);
                }
                else
                {
                    Message = $"Project_stats.txt already exists. Appending...";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error has occured while creating or writing to a dataset file: {e.Message}");
                Constants.log.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Given a project retrieve its metrics
        /// </summary>
        /// <returns>void</returns>
        /// <exception cref="ApiException">Thrown by Octokit</exception>
        /// <exception cref="Exception">Thrown by GetCommits method</exception>
        private async Task<RepositoryIndicators> RetrieveProjectsFromUserAndRepo(string project)
        {
            // username, reponame, number of releases
            string username = "", repo ="";
            Util.HelperFunctions.ExtractUserAndRepoFromPath(project, out username, out repo);
            // check if it's already in the dataset
            string dataSetContents = File.ReadAllText($"Project_stats.txt");
            if (dataSetContents.Contains($@",{username}/{repo},"))
            {
                Message = $@"The data for {username}/{repo} has been collected. Skipping...";
                throw new Exception();
            }

            Message = $"Downloading stats for: {username}/{repo}";

            // get project's indicators, list of contribs, total commits  
            Repository projectStats = null;
            IReadOnlyList<RepositoryContributor> projectContribs = null;
            int numOfCommits = 0, numOfReleases = 0;

            try
            {
                projectStats = await Authenticator.client.Repository.Get(username, repo);
                projectContribs = await Authenticator.client.Repository.GetAllContributors(username, repo);

                var tmp = new ProjectContributorsRetrieval();
                numOfCommits = await tmp.GetTotalCommitsForProject(username, repo);
                numOfReleases = await tmp.GetTotalReleasesForProject(username, repo);
            }
            catch (ApiException e)
            {
                Message = $"An exception has occured: {e.Message}";
                Constants.log.Error(e.Message);
                throw;
            }
            catch (Exception e)
            {
                Message = $"An exception has occured: {e.Message}";
                Constants.log.Error(e.Message);
                throw;
            }
            
            // pack it all up and return for the next step
            return new RepositoryIndicators
            {
                RepoStats = projectStats,
                NumOfContributors = projectContribs.Count,
                NumOfReleases = numOfReleases,
                NumOfCommits = numOfCommits
            };
        }

        /// <summary>
        /// Writes the stats to a file
        /// </summary>
        /// <param name="projectStats">Project statistics</param>
		private void WriteStatsToFile(RepositoryIndicators projectStats)
        {
            // unpack the parameter
            var repositoryStats = projectStats.RepoStats;
            var numOfContribs = projectStats.NumOfContributors;
            var numOfReleases = projectStats.NumOfReleases;
            var numOfCommits = projectStats.NumOfCommits;

            // write it all into a file
            using (var dataSet = new StreamWriter($"Project_stats.txt", true))
            {
                try
                {
                    dataSet.Write($"{repositoryStats.Id},");
                    dataSet.Write($"{repositoryStats.Name},");
                    dataSet.Write($"{repositoryStats.FullName},");
                    dataSet.Write($"{repositoryStats.Url},");
                    dataSet.Write($"{repositoryStats.StargazersCount},");
                    dataSet.Write($"{repositoryStats.SubscribersCount},");
                    dataSet.Write($"{numOfContribs},");
                    dataSet.Write($"{numOfReleases},");
                    dataSet.Write($"{numOfCommits},");
                    dataSet.Write($"{repositoryStats.CreatedAt},");
                    dataSet.Write($"{repositoryStats.ForksCount},");
                    dataSet.Write($"{repositoryStats.Language},");
                    dataSet.Write($"{repositoryStats.HasDownloads},");
                    dataSet.Write($"{repositoryStats.HasIssues},");
                    dataSet.Write($"{repositoryStats.HasPages},");
                    dataSet.Write($"{repositoryStats.HasWiki},");
                    dataSet.Write($"{repositoryStats.Size}");
                    dataSet.Write(Environment.NewLine);
                    dataSet.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error has occured while writing to a dataset file: {e}");
                    Constants.log.Error(e.Message);
                    dataSet.Flush();
                    return;
                } 
            }
        }

        /// <summary>
        /// Run the program
        /// </summary>
        /// <returns>void</returns>
        public async Task RunCollection()
		{
            Message = "Started collection";
            try
            {
                Prepare();
                CreateDataset();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while creating or reading a dataset: " + e.Message);
                throw;
            }
            Authenticator.Authenticate();
            // iterate over list of projects
            foreach (var project in m_listOfProjects)
            {
                // check if we need to collect it

                RepositoryIndicators projectStats = null;
                try
                {
                    projectStats = await RetrieveProjectsFromUserAndRepo(project);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong: " + e.Message);
                    continue;
                }
                WriteStatsToFile(projectStats); 
            }
		}

        /// <summary>
        /// Used temporarily to hold project data
        /// </summary>
        private class RepositoryIndicators
        {
            public Repository RepoStats;
            public int NumOfContributors;
            public int NumOfReleases;
            public int NumOfCommits;
        }
    }
}
