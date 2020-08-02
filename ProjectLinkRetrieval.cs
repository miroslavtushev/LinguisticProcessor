using System;
using System.IO;
using Octokit;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// This class retrieves the top-n links to projects based on the star-rating
    /// </summary>
    public class ProjectLinkRetrieval : INotifyPropertyChanged
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
        /// <summary>
        /// The programming language to retrieve the projects for
        /// </summary>
        private Language m_progLang;

        /// <summary>
        /// The number of links requested by the user
        /// </summary>
        private int m_numOfLinksRequested;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="numOfLinksRequested">The number of links needed to be retrieved</param>
        /// <param name="progLang">The target programming language of the proejcts</param>
        public ProjectLinkRetrieval(int numOfLinksRequested, ProgrammingLanguage progLang)
        {
            m_numOfLinksRequested = numOfLinksRequested;
            if (progLang == ProgrammingLanguage.Java)
            {
                m_progLang = Language.Java;
            }
            else if (progLang == ProgrammingLanguage.CSharp)
            {
                m_progLang = Language.CSharp;
            }
            else if (progLang == ProgrammingLanguage.Python)
            {
                m_progLang = Language.Python;
            }
            else if (progLang == ProgrammingLanguage.JavaScript)
            {
                m_progLang = Language.JavaScript;
            }
            else if (progLang == ProgrammingLanguage.Go)
            {
                m_progLang = Language.Go;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The main function that retrieves the links
        /// </summary>
        /// <returns></returns>
        private async Task RetrieveLinks()
        {
            var request = new SearchRepositoriesRequest()
            {
                Language = m_progLang,
                SortField = RepoSearchSort.Stars
            };
            request.PerPage = 100;
            SearchRepositoryResult repos = null;

            

            // calculate the number of pages needed, given 100 repos per page
            int totalPagesNeeded = (m_numOfLinksRequested + 100 - 1) / 100;
            // collect and write totalPagesNeeded*100 repos to a file
            Message = $@"Saving {m_numOfLinksRequested} links into a file...";
            // workaround to search past 1000 limit
            int curPage = 1;
            if (totalPagesNeeded > 10)
            {
                Message = $@"You are searching past the 1000 limit. Only the results from 1000 to {m_numOfLinksRequested} will be returned (up to 2000).";
                // get the last available page
                repos = await RetrieveLinksHelper(request, 10);
                // get the last repo
                int numStars = repos.Items[repos.Items.Count - 1].StargazersCount;
                // run a search PAST that star number
                request.Stars = new Range(0, numStars);
                totalPagesNeeded -= 10;
            }
            // if user requested <100 links
            if (totalPagesNeeded == curPage)
            {
                repos = await RetrieveLinksHelper(request, curPage);
                Message = $@"Writing page 1";
                WriteToFile(repos, m_numOfLinksRequested);
            }
            else
            {
                for (int i = curPage; i < totalPagesNeeded; i++)
                {
                    repos = await RetrieveLinksHelper(request, i);
                    Message = $@"Writing page {i}";
                    WriteToFile(repos);
                    curPage = i;
                }
                // write the remaining numOfLinksRequested-(totalPagesNeeded*100) to a file
                repos = await RetrieveLinksHelper(request, curPage + 1);
                Message = $@"Writing last page";
                if (m_numOfLinksRequested % 100 == 0)
                {
                    WriteToFile(repos, 100);
                }
                else
                {
                    WriteToFile(repos, m_numOfLinksRequested % 100);
                } 
            }
        }

        /// <summary>
        /// The helper function to retrieve links
        /// </summary>
        /// <param name="request">The Octokit request</param>
        /// <param name="curPage">Current page (100 links per page)</param>
        /// <returns></returns>
        private async Task<SearchRepositoryResult> RetrieveLinksHelper(SearchRepositoriesRequest request, int curPage)
        {
            request.Page = curPage;
            return await Authenticator.client.Search.SearchRepo(request);
        }

        /// <summary>
        /// Writes the links to a file
        /// </summary>
        /// <param name="repos">The Octokit list of repositories</param>
        private void WriteToFile(SearchRepositoryResult repos)
        {
            if (repos != null)
            {
                try
                {
                    using (StreamWriter file = new StreamWriter("links.txt", true))
                    {
                        foreach (var item in repos.Items)
                        {
                            file.WriteLine(item.HtmlUrl);
                        }
                    }
                }
                catch (Exception e)
                {
                    Message = $"An error occured while saveing results to a file: {e}";
                    return;
                }
            }
        }

        /// <summary>
        /// Writes the links to a file
        /// </summary>
        /// <param name="repos">The Octokit list of repositories</param>
        /// <param name="numOfReposToWrite">The remaining number of repositories to write from the list</param>
        private void WriteToFile(SearchRepositoryResult repos, int numOfReposToWrite)
        {
            if (repos != null)
            {
                try
                {
                    using (StreamWriter file = new StreamWriter("links.txt", true))
                    {
                        for (int i = 0; i < numOfReposToWrite; i++)
                        {
                            file.WriteLine(repos.Items[i].HtmlUrl);
                        }
                    }
                }
                catch (Exception e)
                {
                    Message = $"An error occured while saveing results to a file: {e}";
                    return;
                }
            }
        }

        /// <summary>
        /// Runs the program
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            Authenticator.Authenticate();
            await RetrieveLinks();
            Message = "Done";
        }
    }
}
