using Program;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// Interaction logic for WordExtractorMultipleWindow.xaml
    /// </summary>
    public partial class ProjectsStatsWindow : Window
    {
        public string SelectedProjects { get; set; }
        public ProgrammingLanguage SelectedLanguage { get; set; }
        public bool OverwriteDataset { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProjectsStatsWindow()
        {
            InitializeComponent();
            DataContext = this;
            StatusBar.Visibility = Visibility.Visible;

            // populate drop-down
            LanguageSelector.ItemsSource = Enum.GetValues(typeof(ProgrammingLanguage));
            LanguageSelector.SelectedValue = LanguageSelector.Items[0];
        }

        #region Menu items
        /// <summary>
        /// Opens Word Extractor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordExtractor_click(object sender, RoutedEventArgs e)
        {
            var input = Util.HelperFunctions.ShowChangeWindowBox();
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
            var input = Util.HelperFunctions.ShowChangeWindowBox();
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
            var input = Util.HelperFunctions.ShowChangeWindowBox();
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                ProjectsStatsWindow m = new ProjectsStatsWindow();
                m.Show();
                this.Close();
            }
        }
        #endregion

        #region Buttons    
        private void StartCollectionStats_click(object sender, RoutedEventArgs e)
        {
            WordExtractorW.IsEnabled = false;
            StartGHAsync();
        }

        private void SpecifyProjectPath_click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            FolderProjects.Foreground = Brushes.Black;
            FolderProjects.IsEnabled = true;
            FolderProjects.Text = folderBrowserDialog.SelectedPath;

            StartCollectingLocalStats_bt.IsEnabled = true;
            StartCollectingStats_bt.IsEnabled = false;
        }

        private void StartCollectionLocalStats_click(object sender, RoutedEventArgs e)
        {
            SelectedProjects = FolderProjects.Text;
            WordExtractorW.IsEnabled = false;
            StartDescrAsync();
        }
        #endregion

        #region Checkboxes
        private void Overwrite_checked(object sender, RoutedEventArgs e)
        {
            OverwriteDataset = true;
        }

        private void Overwrite_unchecked(object sender, RoutedEventArgs e)
        {
            OverwriteDataset = false;
        }
        #endregion

        private void LanguageSelector_changed(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            SelectedLanguage = (ProgrammingLanguage)comboBox.SelectedItem;
        }

        /// <summary>
        /// Collects GH stats for all projects
        /// </summary>
        public async void StartGHAsync()
        {
            var sr = new ProjectStatsRetrieval();
            DataContext = sr;
            sr.OverwriteDataset = OverwriteDataset;

            try
            {
                await Task.Run(() => sr.RunCollection());

            }
            catch (Exception e)
            {
                //ShowMessageBox(e.Message);
                return;
            }
            WordExtractorW.IsEnabled = true;
        }

        /// <summary>
        /// Calculates descriptive stats for Fig.1 for specified language
        /// </summary>
        public async void StartDescrAsync()
        {
            var ds = DescriptiveStatistics.WithProjectsPath(SelectedLanguage, SelectedProjects);
            DataContext = ds;
            string _ret = "";
            await Task.Run(() => _ret = ds.Run());
            StatusBlock.Text = "Done. Results saved in: " + _ret;
            WordExtractorW.IsEnabled = true;
        }
    }
}