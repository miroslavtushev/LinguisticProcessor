using Program;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// Interaction logic for LinguisticChange.xaml
    /// </summary>
    public partial class LinguisticChangeMultipleWindow : Window
    {
        public bool IsAverage { get; set; }
        public bool IsWordTypes { get; set; }
        public string SelectedProject { get; set; }
        public LinguisticChangeMultipleWindow()
        {
            InitializeComponent();
            DataContext = this;
            StatusBar.Visibility = Visibility.Visible;
        }

        private void FirstAndLastBrowse_click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;
                FolderProjects.Foreground = Brushes.Black;
                FolderProjects.IsEnabled = true;
                FolderProjects.Text = folderBrowserDialog.SelectedPath;

                SeveralProjectsStartCalculation_bt.IsEnabled = true;
            }
        }

        private void InterquartileBrowse_click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;
                FolderProjects1.Foreground = Brushes.Black;
                FolderProjects1.IsEnabled = true;
                FolderProjects1.Text = folderBrowserDialog.SelectedPath;

                InterquartileStartCalculation_bt.IsEnabled = true;
            }
        }

        private void SurvivalBrowse_click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;
                FolderProjects2.Foreground = Brushes.Black;
                FolderProjects2.IsEnabled = true;
                FolderProjects2.Text = folderBrowserDialog.SelectedPath;

                SurvivalStartCalculation_bt.IsEnabled = true;
            }
        }

        private void SeveralProjectsStartCalculation_click(object sender, RoutedEventArgs e)
        {
            CalculateBetweenFirstAndLast();
        }

        private void InterquartileStartCalculation_click(object sender, RoutedEventArgs e)
        {
            CalculateInterquartile();
        }

        private void SurvivalStartCalculation_click(object sender, RoutedEventArgs e)
        {
            CalculateSyntacticFitness();
        }

        private void CalculateAverages_checked(object sender, RoutedEventArgs e)
        {
            IsAverage = true;
        }

        private void CalculateAverages_unchecked(object sender, RoutedEventArgs e)
        {
            IsAverage = false;
        }

        private void CalculateWordTypes_checked(object sender, RoutedEventArgs e)
        {
            IsWordTypes = true;
        }

        private void CalculateWordTypes_unchecked(object sender, RoutedEventArgs e)
        {
            IsWordTypes = false;
        }

        private void CalculateWordLengths_checked(object sender, RoutedEventArgs e)
        {
            IsWordTypes = false;
        }

        private void CalculateWordLengths_unchecked(object sender, RoutedEventArgs e)
        {
            IsWordTypes = true;
        }

        public async void CalculateBetweenFirstAndLast()
        {
            SelectedProject = FolderProjects.Text;
            LinguisticChangeW.IsEnabled = false;
            try
            {
                var lc = new LinguisticChangeCalculator();
                await Task.Run(() => lc.LinguisticChangeAllProjectsToFile(SelectedProject));
                //await Task.Run(() => lc.WriteLinguisticChangeQuartileCodeForEachProjectToFile(SelectedProject));
            }
            catch (Exception ex)
            {
                Util.HelperFunctions.ShowMessageBox("An error occured while calculating linguistic change: " + ex.Message);
                return;
            }
            LinguisticChangeW.IsEnabled = true;
        }

        public async void CalculateInterquartile()
        {
            //IsAverage = UI.LinguisticChangeMultipleWindow._LinguisticChangeMultipleWindow.IsAverage.;
            SelectedProject = FolderProjects1.Text;
            LinguisticChangeW.IsEnabled = false;
            try
            {
                var lc = new LinguisticChangeCalculator();
                await Task.Run(() => lc.WriteLinguisticChangeQuartileCodeForEachProjectToFile(SelectedProject, isAverage: IsAverage));
            }
            catch (Exception ex)
            {
                Util.HelperFunctions.ShowMessageBox("An error occured while calculating linguistic change: " + ex.Message);
                return;
            }
            LinguisticChangeW.IsEnabled = true;
        }

        public async void CalculateSyntacticFitness()
        {
            SelectedProject = FolderProjects2.Text;
            LinguisticChangeW.IsEnabled = false;
            try
            {
                var lc = new LinguisticChangeCalculator();
                await Task.Run(() => lc.CalculateSyntacticFitnessForEachProject(SelectedProject, IsWordTypes));
            }
            catch (Exception ex)
            {
                Util.HelperFunctions.ShowMessageBox("An error occured while calculating linguistic change: " + ex.Message);
                return;
            }
            LinguisticChangeW.IsEnabled = true;
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
    }
}
