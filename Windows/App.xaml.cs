using System.Windows;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Menu items
        /// <summary>
        /// Opens Word Extractor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WordExtractor_click(object sender, RoutedEventArgs e)
        {
           
        }

        /// <summary>
        /// Opens Linguistic Change calculator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LinguisticChange_click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Opens Projects Stats retrieval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ProjectsStatistics_click(object sender, RoutedEventArgs e)
        {
            //var input = ViewModel.ShowChangeWindowBox();
            //if (input == System.Windows.Forms.DialogResult.Yes)
            //{
            //    ProjectsStatsWindow m = new ProjectsStatsWindow();
            //    m.Show();
            //    this.Close();
            //}
        }
        #endregion

        private void App_Startup(object sender, StartupEventArgs e)
        {
            ProjectsDownloaderWindow projectDownloader = new ProjectsDownloaderWindow();
            projectDownloader.Show();
        }
    }
}
