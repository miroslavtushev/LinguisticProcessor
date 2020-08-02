using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SEEL.LinguisticProcessor;
using SEEL.LinguisticProcessor.Exceptions;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// Interaction logic for WordExtractorMultipleWindow.xaml
    /// </summary>
    public partial class WordExtractorMultipleWindow : Window
    {
        /// <summary>
        /// Holds a path to projects
        /// </summary>
        private string SelectedFolderWithProjects { get; set; }
        public bool IsCamelCase { get; set; }
        public bool IsUnderscore { get; set; }
        public bool IsCodeAsNatural { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public WordExtractorMultipleWindow()
        {
            InitializeComponent();
            DataContext = this;
            StatusBar.Visibility = Visibility.Visible;
        }

        #region Menu items
        /// <summary>
        /// Opens Word Extractor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordExtractor_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult input = System.Windows.Forms.DialogResult.Yes;// ViewModel.ShowChangeWindowBox();
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                WordExtractorWindow m = new WordExtractorWindow();
                m.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Opens Linguistic Change calculator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinguisticChange_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult input = System.Windows.Forms.DialogResult.Yes;// ViewModel.ShowChangeWindowBox();
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                LinguisticChangeSingleWindow m = new LinguisticChangeSingleWindow();
                m.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Opens Projects Stats retrieval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectsStatistics_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult input = System.Windows.Forms.DialogResult.Yes;// ViewModel.ShowChangeWindowBox();
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                ProjectsStatsWindow m = new ProjectsStatsWindow();
                m.Show();
                this.Close();
            }
        }
        #endregion

        #region Buttons
        private void SpecifyProjectPath_click(object sender, RoutedEventArgs e)
        {

            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;

                FolderProjects.Foreground = Brushes.Black;
                FolderProjects.IsEnabled = true;
                FolderProjects.Text = folderBrowserDialog.SelectedPath;

                StartWordExtraction_bt.IsEnabled = true;
            }
        }

        private async void StartWordExtraction_click(object sender, RoutedEventArgs e)
        {
            WordExtractorW.IsEnabled = false;
            await Start(); ///////////////////////
            WordExtractorW.IsEnabled = true;
        }
        #endregion

        #region Checkboxes
        private void CamelCaseSplitting_checked(object sender, RoutedEventArgs e)
        {
            IsCamelCase = true;
        }

        private void UnderscoreSplitting_checked(object sender, RoutedEventArgs e)
        {
            IsUnderscore = true;
        }

        private void CamelCaseSplitting_unchecked(object sender, RoutedEventArgs e)
        {
            IsCamelCase = false;
        }

        private void UnderscoreSplitting_unchecked(object sender, RoutedEventArgs e)
        {
            IsUnderscore = false;
        }

        private void IsCodeAsNatural_checked(object sender, RoutedEventArgs e)
        {
            IsCodeAsNatural = true;
        }

        private void IsCodeAsNatural_unchecked(object sender, RoutedEventArgs e)
        {
            IsCodeAsNatural = false;
        }
        #endregion

        public async Task Start()
        {
            // get input
            SelectedFolderWithProjects = FolderProjects.Text;
            // locate projects
            List<string> ProjectFolders = Directory.GetDirectories(SelectedFolderWithProjects).ToList();
            if (ProjectFolders.Count == 0)
            {
                //ShowMessageBox("No projects have been found!");
                return;
            }

            foreach (var project in ProjectFolders)
            {
                NamesExtractors.BaseNamesExtractor extractor = null;
                try
                {
                    (extractor, _) = Util.HelperFunctions.IdentifyNamesExtractor(project);
                }
                catch
                {
                    //ShowMessageBox($"Identifynames in NamesExtractor failed for project: {project}");
                    continue;
                }

                // extractor setup
                extractor.IsCamelCase = IsCamelCase;
                extractor.IsUnderscore = IsUnderscore;
                DataContext = extractor;
                try
                {
                    await Task.Run(() => extractor.Run());
                }
                catch (NoReleasesFoundException)
                {
                    //ShowMessageBox("No releases have been found");
                    return;
                }
            }
        }
    }
}
