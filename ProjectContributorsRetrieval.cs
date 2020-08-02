using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEEL.LinguisticProcessor
{
	/// <summary>
    /// This class retrives the committers to a projects and writes them into a file
    /// </summary>
    public class ProjectContributorsRetrieval
    {
		/// <summary>
        /// Path to a project
        /// </summary>
		private string m_pathToProject;
        /// <summary>
        /// Holds retrieved releases
        /// </summary>
		private List<Release> m_releases = null;
        /// <summary>
        /// Holds retrieved commits
        /// </summary>
		private List<Commit> m_commits = null;

        public ProjectContributorsRetrieval(string pathToProject)
        {
			m_pathToProject = pathToProject;
            //Run();
        }

        public ProjectContributorsRetrieval() { }
        
        /// <summary>
        /// Gets all commits given a link to a project
        /// </summary>
        /// <returns></returns>
        private async Task GetAllCommits(string pathToProject)
		{
			string user = "", repo = "";
			Util.HelperFunctions.ExtractUserAndRepoFromPath(pathToProject, out user, out repo);

			IReadOnlyList<Octokit.GitHubCommit> allCommits = null;

            allCommits = await Authenticator.client.Repository.Commit.GetAll(user, repo);
	        Console.WriteLine($@"Found {allCommits.Count} commits");
			m_commits = ConvertCommits(allCommits);
		}

        /// <summary>
        /// Gets all releases given a path to a project
        /// </summary>
        /// <returns></returns>
		private async Task GetAllReleases(string pathToProject)
		{
			string user = "", repo = "";
            Util.HelperFunctions.ExtractUserAndRepoFromPath(pathToProject, out user, out repo);

			IReadOnlyList<Octokit.Release> allReleases = null;
            //try 
			//{
				allReleases = await GetAllReleasesHelper();
				Console.WriteLine($@"Found {allReleases.Count} releases");
				m_releases = ConvertReleases(allReleases);
				//GroupCommitsByReleases(releaseDatesList, null);

			//}
			//catch (Exception e)
            ////{
            //    Console.WriteLine($"An exception has occured: {e}. Perhaps, the username or repo you've provided is broken or in the wrong format");
            //    Constants.log.Error(e.Message);
            //    return;
            //}

			async Task<IReadOnlyList<Octokit.Release>> GetAllReleasesHelper()
            {
                return await Task.Run(() => Authenticator.client.Repository.Release.GetAll(user, repo));
            }
		}

        /// <summary>
        /// Converts the Octokit.Release into my Release class
        /// </summary>
        /// <returns>The list of converted releases</returns>
        /// <param name="allReleases">The list of Octokit.Releases</param>
		private List<Release> ConvertReleases(IReadOnlyList<Octokit.Release> allReleases)
		{
			List<Release> ret = new List<Release>();
            
			var first = allReleases[allReleases.Count-1];
			for (int i = 0; i < allReleases.Count; i++)
			{
				if (allReleases[i] == first)
				{
					ret.Add(new Release(allReleases[i].Id, new DateTimeOffset(DateTime.MinValue.AddYears(1)), allReleases[i].CreatedAt));
				}
				else
				{
					ret.Add(new Release(allReleases[i].Id, allReleases[i + 1].CreatedAt, allReleases[i].CreatedAt));
                }
			}
			return ret;
		}
        
		/// <summary>
        /// Converts the Octokit.GitHubCommit into my Commit class
        /// </summary>
        /// <returns>The list of converted commits</returns>
        /// <param name="allCommits">The list of Octokit.GitHubCommits</param>
		private List<Commit> ConvertCommits(IReadOnlyList<Octokit.GitHubCommit> allCommits)
        {
			List<Commit> ret = new List<Commit>();

			foreach (var c in allCommits)
			{
				if (c.Author != null)
				{
					ret.Add(new Commit(c.Author.Id, c.Commit.Author.Date));
				}
			}
			return ret;
        }
        
        /// <summary>
        /// Prints the list of commits
        /// </summary>
        /// <param name="list">List of commits</param>
		private void PrintCommits(List<Commit> list)
		{
            foreach (var elem in list)
			{
				elem.Print();
			}
		}
        
        /// <summary>
        /// Prints the list of releases
        /// </summary>
        /// <param name="list">List of releases</param>
		private void PrintReleases(List<Release> list)
        {
            foreach (var elem in list)
            {
				elem.Print();
			}
        }

        /// <summary>
        /// Groups all the commits by releases
        /// </summary>
        /// <returns>The list of commits by releases</returns>
        /// <param name="releases">List of releases</param>
        /// <param name="commits">List of commits</param>
		private List<Tuple<Release, List<Commit>>> GroupCommitsByReleasesFull(List<Release> releases, List<Commit> commits)
		{
			// release 1:
            //   - 8899 commited on 3/4/12
            //   - 8899 commited on 3/5/12
            //   - 76652 commited on ...
			// release 2:
            // ...
			List<Tuple<Release, List<Commit>>> ret = new List<Tuple<Release, List<Commit>>>();

			for (int i = 0; i < releases.Count; i++)
			{
				List<Commit> listOfCommits = new List<Commit>();
				for (int j = 0; j < commits.Count; j++)
				{
					if (commits[j].DateOfCommit > releases[i].StartDevelopment && commits[j].DateOfCommit < releases[i].PublishedAt)
					{
						listOfCommits.Add(commits[j]);
					}
				}
				ret.Add(new Tuple<Release, List<Commit>>(releases[i], listOfCommits));
			}
			ret.Reverse();
			return ret; 
		}

        /// <summary>
        /// Groups the unique committers by releases
        /// </summary>
        /// <returns>The list of unique commits by releases</returns>
		public List<Tuple<Release, List<int>>> GroupCommitsByReleasesUnique()
		{
			List<Tuple<Release, List<int>>> ret = new List<Tuple<Release, List<int>>>();

			for (int i = 0; i < m_releases.Count; i++)
            {
                List<int> listOfCommitters = new List<int>();
                for (int j = 0; j < m_commits.Count; j++)
                {
					if (!listOfCommitters.Contains(m_commits[j].Id) && m_commits[j].DateOfCommit > m_releases[i].StartDevelopment && m_commits[j].DateOfCommit < m_releases[i].PublishedAt)
                    {
                        listOfCommitters.Add(m_commits[j].Id);
                    }
                }
                ret.Add(new Tuple<Release, List<int>>(m_releases[i], listOfCommitters));
            }
			ret.Reverse();
            return ret; 
		}

        /// <summary>
        /// Writes the releases and commits with dates to a file
        /// </summary>
        /// <param name="path">Path to a file</param>
        /// <param name="data">List of releases and commits</param>
		private void WriteGrouppedCommitsToFile(string path, List<Tuple<Release, List<Commit>>> data)
		{
			using (var file = new StreamWriter(path))
            {
                for (int i = 0; i < data.Count; i++)
                {
                    file.WriteLine($@"{data[i].Item1.Id} is valid from {data[i].Item1.StartDevelopment} until {data[i].Item1.PublishedAt}");
                    foreach (var f in data[i].Item2)
                    {
                        file.WriteLine($@"{f.Id} commited on {f.DateOfCommit}");
                    }

                }
            }
		}

        /// <summary>
        /// Writes username and repo to a file along with total # of commits for given # of releases, to a dataset
        /// </summary>
        /// <param name="userAndRepo">username/repo</param>
        /// <param name="numOfReleases">number of releases to count</param>
        /// <param name="data">releases and commits</param>
        private void WriteTotalCommitsBasedOnReleasesToFile(string userAndRepo, int numOfReleases, List<Tuple<Release, List<Commit>>> data)
        {
            using (var file = new StreamWriter($@"{Constants.PROJECTS_PATH}\CommitData.txt", true))
            {
                int total = 0;
                file.Write($@"{userAndRepo},");
                for (int i = 0; i < numOfReleases; i++)
                {
                    total += data[i].Item2.Count();
                }
                file.Write($@"{total}");
                file.Write(Environment.NewLine);
            }
        }

        public void WriteCommitDataToFile(string path, List<Tuple<Release, List<Commit>>> data)
        {
            // find how many releases need to extract based on path
            int relToExtr = Directory.GetDirectories(path + @"/src/").Count();
            // extract username and repo
            string user, repo;
            Util.HelperFunctions.ExtractUserAndRepoFromPath(path, out user, out repo);
            // run the main function
            WriteTotalCommitsBasedOnReleasesToFile($@"{user}/{repo}", relToExtr, data);
        }

        /// <summary>
        /// Writes the unique committers for each release into a dataset
        /// </summary>
        /// <param name="path">Path to a dataset</param>
        /// <param name="project">Project.</param>
        /// <param name="data"><list type=" of releases and unique commiters' ids"</param>
		private void WriteUniqueCommittersDataSet(string path, string project, List<Tuple<Release, List<int>>> data)
		{
			var _path = path + project.Replace(@"\", "_") + "_Contribs.txt";
			Console.WriteLine($@"Creating file {_path}...");
			using (var dataSet = new StreamWriter(_path))
            {
				try
                {
                    dataSet.Write($@"releaseNum,devsAll,devsLeft,devsNew,devsDelta");
                    dataSet.Write(Environment.NewLine);
					for (int i = 0; i < data.Count; i++)
                    {
                        dataSet.Write($@"{i+1},");
                        // total # of developers for that release
						dataSet.Write($@"{data[i].Item2.Count},");
						// # of devs who left compared to (i-1) release
						int devsLeft = 0;
                        if (i == 0)
						{
							dataSet.Write($@"0,");
						}
						else
						{
							devsLeft = data[i - 1].Item2.Except(data[i].Item2).Count();
							dataSet.Write($@"{devsLeft},");
                        }
						// # of devs who joined compared to (i-1) release
						int devsNew = 0;
						if (i == 0)
                        {
							devsNew = data[i].Item2.Count;
							dataSet.Write($@"{devsNew},");
                        }
                        else
                        {
							devsNew = data[i].Item2.Except(data[i - 1].Item2).Count();
                            dataSet.Write($@"{devsNew},");
                        }
                        // sum of previous two
						dataSet.Write($@"{devsLeft + devsNew}");
						dataSet.Write(Environment.NewLine);
						dataSet.Flush();
                    }
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
        /// Runs the program
        /// </summary>
        /// <returns>void</returns>
        public async Task Run()
		{
			Authenticator.Authenticate();
            List<string> paths = new List<string>();

            paths.Add(@"Z:\PythonCompressed\lbryio_lbry");

            string contents = "";

            if (File.Exists($@"{Constants.PROJECTS_PATH}\CommitData.txt"))
                contents = File.ReadAllText($@"{Constants.PROJECTS_PATH}\CommitData.txt");

            foreach (var path in paths)
            {
                ///////// check if the data has been collected yet
                string user, repo;
                Util.HelperFunctions.ExtractUserAndRepoFromPath(path, out user, out repo);
                if (Regex.IsMatch(contents, $@"{user}/{repo}"))
                {
                    Console.WriteLine($@"{user}/{repo} is already collected. Skipping...");
                    continue;
                }
                //////////
                try
                {
                    await GetAllReleases(path); // saves into m_releases
                    await GetAllCommits(path); // saves into m_commits
                                               //var b = GroupCommitsByReleasesUnique(m_releases, m_commits);
                    var c = GroupCommitsByReleasesFull(m_releases, m_commits);
                    WriteCommitDataToFile(path, c);
                }
                catch
                {
                    Console.WriteLine($@"An exception has occured while processing: {path}");
                    continue;
                }
            }
			//WriteUniqueCommittersDataSet($@"Z:\CSharpCompressed\akkadotnet_akka.net", @"akkadotnet\akka.net", b);
			//WriteGrouppedCommitsToFile($"/Users/miroslav/Example/Contrib.txt", c);
		}

        public async Task<int> GetTotalCommitsForProject(string username, string repo)
        {
            IReadOnlyList<Octokit.GitHubCommit> commits = null;
            try
            {
                commits =  await Authenticator.client.Repository.Commit.GetAll(username, repo); 
            }
            catch
            {
                Console.WriteLine($@"An exception has occured while processing: {username}/{repo}");
                throw new Exception($@"Exception was thrown while obtaining commits for {username}/{repo}");
            }
            return commits.Count();
        }

        public async Task<int> GetTotalReleasesForProject(string username, string repo)
        {
            IReadOnlyList<Octokit.Release> releases = null;
            try
            {
                releases = await Authenticator.client.Repository.Release.GetAll(username, repo);
            }
            catch
            {
                Console.WriteLine($@"An exception has occured while processing: {username}/{repo}");
                throw new Exception($@"Exception was thrown while obtaining commits for {username}/{repo}");
            }
            return releases.Count();
        }
    }
}
