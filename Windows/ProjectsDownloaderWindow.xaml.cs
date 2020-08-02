using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// Interaction logic for WordExtractorMultipleWindow.xaml
    /// </summary>
    public partial class ProjectsDownloaderWindow : Window
    {
        public ProgrammingLanguage SelectedLanguage { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProjectsDownloaderWindow()
        {
            InitializeComponent();
            StatusBar.Visibility = Visibility.Visible;
            StartButtonDownload.IsEnabled = false;
            StartButtonLinks.IsEnabled = false;

            // populate drop-down
            LanguageSelector.ItemsSource = Enum.GetValues(typeof(ProgrammingLanguage));
            LanguageSelector.SelectedValue = LanguageSelector.Items[0];

            LanguageSelectorLinks.ItemsSource = Enum.GetValues(typeof(ProgrammingLanguage));
            LanguageSelectorLinks.SelectedValue = LanguageSelectorLinks.Items[0];
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
                Close();
            }
        }

        /// <summary>
        /// Opens Linguistic Change calculator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinguisticChange_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult input = System.Windows.Forms.DialogResult.Yes;//ViewModel.ShowChangeWindowBox();
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
            System.Windows.Forms.DialogResult input = System.Windows.Forms.DialogResult.Yes;//ViewModel.ShowChangeWindowBox();
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                ProjectsStatsWindow m = new ProjectsStatsWindow();
                m.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Opens Projects Downloader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectsDownloader_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult input = System.Windows.Forms.DialogResult.Yes;//ViewModel.ShowChangeWindowBox();
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                ProjectsDownloaderWindow m = new ProjectsDownloaderWindow();
                m.Show();
                Close();
            }
        }
        #endregion

        #region Buttons    
        private async void Start_click(object sender, RoutedEventArgs e)
        {
            var SelectedTEXTFile = TxtProjects.Text;
            var p = new ProjectDownloader(SelectedTEXTFile, SelectedLanguage);
            DataContext = p;
            ProjectDownloaderW.IsEnabled = false;
            await p.Run();
            ProjectDownloaderW.IsEnabled = true;
        }

        private void SpecifyTXTFilePath_click(object sender, RoutedEventArgs e)
        {

            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;

                TxtProjects.Foreground = Brushes.Black;
                TxtProjects.IsEnabled = true;
                TxtProjects.Text = openFileDialog.FileName;

                StartButtonDownload.IsEnabled = true;
            }       
        }

        private async void StartLinks_click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(NumOfLinks.Text, out int nlinks))
                nlinks = 10;
            var p = new ProjectLinkRetrieval(nlinks, SelectedLanguage);
            DataContext = p;
            ProjectDownloaderW.IsEnabled = false;
            await p.Run();
            ProjectDownloaderW.IsEnabled = true;
        }
        #endregion

        private void LanguageSelector_changed(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            SelectedLanguage = (ProgrammingLanguage)comboBox.SelectedItem;
        }

        private void LanguageSelectorLinks_changed(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            SelectedLanguage = (ProgrammingLanguage)comboBox.SelectedItem;
            StartButtonLinks.IsEnabled = true;
        }
    }
}

