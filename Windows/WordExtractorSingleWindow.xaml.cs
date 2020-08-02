using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SEEL.LinguisticProcessor.Exceptions;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// Interaction logic for WordExtractorWindow.xaml
    /// </summary>
    public partial class WordExtractorWindow : Window
    {
        #region Input options
        public bool IsCalculateStatistics { get; set; }
        public bool IsCamelCase { get; set; }
        public bool IsUnderscore { get; set; }
        public DescriptiveStatistics Statistics { get; set; }
        private string ProgrammingLanguage { get; set; }
        public NamesExtractors.BaseNamesExtractor Extractor { get; set; }
        public string SelectedProject { get; set; }
        #endregion
        public WordExtractorWindow()
        {
            InitializeComponent();
            StatusBar.Visibility = Visibility.Visible;
            StatisticsBar.Visibility = Visibility.Hidden;
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
            System.Windows.Forms.DialogResult input = System.Windows.Forms.DialogResult.Yes;
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                ProjectsStatsWindow m = new ProjectsStatsWindow();
                m.Show();
                this.Close();
            }
        }

        private void ProjectsDownloader_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult input = System.Windows.Forms.DialogResult.Yes;
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                ProjectsDownloaderWindow m = new ProjectsDownloaderWindow();
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
                ProjectPath.Foreground = Brushes.Black;
                ProjectPath.IsEnabled = true;
                ProjectPath.Text = folderBrowserDialog.SelectedPath;
            }
            StartWordExtraction_bt.IsEnabled = true;
        }

        /// <summary>
        /// Opens window with multiple projects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpecifyProjectsFolderPath_click(object sender, RoutedEventArgs e)
        {
            WordExtractorMultipleWindow m = new WordExtractorMultipleWindow();
            m.Show();
            Close();
        }

        private void StartWordExtraction_click(object sender, RoutedEventArgs e)
        {
            StatusBar.Visibility = Visibility.Visible;
            StatisticsBar.Visibility = Visibility.Hidden;

            // remove previous tabs
            OutputTab.Items.Clear();

            TabItem words_code = new TabItem() { Header = "Words Code", Content = new TextBox() };
            TabItem words_comments = new TabItem() { Header = "Words Comments", Content = new TextBox() };
            TabItem acronyms_code = new TabItem() { Header = "Acronyms Code", Content = new TextBox() };
            TabItem acronyms_comments = new TabItem() { Header = "Acronyms Comments", Content = new TextBox() };
            TabItem misspellings_code = new TabItem() { Header = "Misspellings Code", Content = new TextBox() };
            TabItem misspellings_comments = new TabItem() { Header = "Misspellings Comments", Content = new TextBox() };
            TabItem unident_code = new TabItem() { Header = "Unidentified Code", Content = new TextBox() };
            TabItem unident_comments = new TabItem() { Header = "Unidentified Comments", Content = new TextBox() };

            OutputTab.Items.Insert(OutputTab.Items.Count, words_code);
            OutputTab.Items.Insert(OutputTab.Items.Count, words_comments);
            OutputTab.Items.Insert(OutputTab.Items.Count, acronyms_code);
            OutputTab.Items.Insert(OutputTab.Items.Count, acronyms_comments);
            OutputTab.Items.Insert(OutputTab.Items.Count, misspellings_code);
            OutputTab.Items.Insert(OutputTab.Items.Count, misspellings_comments);
            OutputTab.Items.Insert(OutputTab.Items.Count, unident_code);
            OutputTab.Items.Insert(OutputTab.Items.Count, unident_comments);

            // make all unclickable
            WordExtractorW.IsEnabled = false;
            Start();
        }
        #endregion

        #region Drow-down box
        private void Releases_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WordExtractorW.IsEnabled = true;

            // remove all previous text
            foreach (TabItem item in OutputTab.Items)
            {
                TextBox t = (TextBox)item.Content;
                t.Clear();
            }

            var comboBox = sender as ComboBox;
            string selected = comboBox.SelectedItem as string;

            // append new text
            foreach (TabItem item in OutputTab.Items)
            {
                TextBox t = (TextBox)item.Content;
                // construct the path in the form of ../r1_acronyms_code.txt
                t.AppendText(File.ReadAllText(Path.Combine(Extractor.CurrentProjectPath,
                   $@"r{Regex.Match(selected, @"\d+").Groups[0].Value}_{item.Header.ToString().ToLower().Replace(' ', '_')}.txt")));
                t.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
        }

        private void OutputTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            var tabitm = e.AddedItems[0] as TabItem;
            TextBox selected = (TextBox)tabitm.Content;
            if (selected == null)
                return;
            NumberOfWords.Text = (selected.LineCount - 1).ToString();
        }
        #endregion

        #region Checkboxes
        private void CalculateStatistics_checked(object sender, RoutedEventArgs e)
        {
            IsCalculateStatistics = true;
        }

        private void CamelCaseSplitting_checked(object sender, RoutedEventArgs e)
        {
            IsCamelCase = true;
        }

        private void UnderscoreSplitting_checked(object sender, RoutedEventArgs e)
        {
            IsUnderscore = true;
        }

        private void CalculateStatistics_unchecked(object sender, RoutedEventArgs e)
        {
            IsCalculateStatistics = false;
        }

        private void CamelCaseSplitting_unchecked(object sender, RoutedEventArgs e)
        {
            IsCamelCase = false;
        }

        private void UnderscoreSplitting_unchecked(object sender, RoutedEventArgs e)
        {
            IsUnderscore = false;
        }
        #endregion

        #region Methods
        private async void Start()
        {
            // get the user input
            SelectedProject = ProjectPath.Text;

            try
            {
                (Extractor, ProgrammingLanguage) = Util.HelperFunctions.IdentifyNamesExtractor(SelectedProject);
            }
            catch (Exception e)
            {
                Util.HelperFunctions.ShowMessageBox(e.Message);
                return;
            }

            Statistics = new DescriptiveStatistics(ProgrammingLanguage, SelectedProject);

            // start calculating
            try
            {
                DataContext = Extractor;
                Extractor.IsCamelCase = IsCamelCase;
                Extractor.IsUnderscore = IsUnderscore;
                WordExtractorW.IsEnabled = false;
                await Extractor.Run();
            }
            catch (NoReleasesFoundException)
            {
                Util.HelperFunctions.ShowMessageBox("No releases have been found");
                return;
            }
            ExtractionCompleted();
        }

        
        private void ExtractionCompleted()
        {
            WordExtractorW.IsEnabled = true;
            var releases = Extractor.Releases;
            // populate the drop-down list
            Releases_cb.ItemsSource = releases.OrderBy(x => int.Parse(Regex.Match(x, @"\d+").Value));
            Releases_cb.SelectedValue = Releases_cb.Items[0];

            // calculate the statistics for the project
            if (IsCalculateStatistics)
            {
                // remove status bar and add statistics bar
                StatusBar.Visibility = Visibility.Hidden;
                StatisticsBar.Visibility = Visibility.Visible;
                CalculateStatisticsForProject(SelectedProject);
            }
        }

        private void CalculateStatisticsForProject(string project)
        {
            DataContext = Statistics;
            FilesTotal.Text = Statistics.CountNumberOfFiles().ToString();
            var temp = Statistics.CountKLOCForProject();
            var conv = new Util.LOCConverter(temp);
            TotalLOC.Text = conv.Convert();
        }
        #endregion
    }
}
