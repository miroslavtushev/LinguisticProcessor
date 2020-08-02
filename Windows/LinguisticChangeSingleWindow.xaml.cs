using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Program;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// Interaction logic for LinguisticChangeSingle.xaml
    /// </summary>
    public partial class LinguisticChangeSingleWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged implementation
        #region Properties
        public string SelectedProject { get; set; }
        private IList<DataPoint> _WordUtilityPoints;
        public IList<DataPoint> WordUtilityPoints { get; set; }
        private PlotModel _model;
        public PlotModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (value != _model)
                {
                    _model = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private List<LineSeries> _LineSeries = new List<LineSeries>();

        public bool IsLingChangeRadio { get; set; }
        public bool IsWordBirthDeathRadio { get; set; }
        public bool IsUniqueCommiters { get; set; }
        public bool IsBetweenFirstAndLast { get; set; }
        /// <summary>
        /// True : CODE/COMMENTS, false : Words
        /// </summary>
        public bool Mode { get; set; }
        #endregion
        public LinguisticChangeSingleWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

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

                CalculateWordUtility.IsEnabled = true;
                OutputWordUtilityBt.IsEnabled = true;
            }
        }

        private void CalculateWordUtility_click(object sender, RoutedEventArgs e)
        {
            StartCalculationAsync();
        }

        public async void StartCalculationAsync()
        {
            SelectedProject = ProjectPath.Text;
            CalculateWordUtility.IsEnabled = false;
            // calculate ling change for this project
            var lc = new LinguisticChangeCalculator();
            await Task.Run(() => lc.WordUtilityCodeCommentsWordsToFile(SelectedProject));
            // output word utility
            CalculateWordUtility.IsEnabled = true;
            OutputWordUtility();
        }

        /// <summary>
        /// Loads txt files with word utility and displays them in <see cref="UI.LinguisticChangeSingleWindow._LinguisticChangeSingleWindow.OutputCode"></see>
        /// </summary>
        public void OutputWordUtility(int releaseNum = 1)
        {
            SelectedProject = ProjectPath.Text;
            // get files
            var codeFiles = GetCodeUtilityFiles(SelectedProject);
            var commentFiles = GetCommentUtilityFiles(SelectedProject);
            string codeFile;
            string commentFile;

            // split
            codeFile = codeFiles.FirstOrDefault(x => Regex.IsMatch(x, @"code" + releaseNum + @"Ling\.txt"));
            commentFile = commentFiles.FirstOrDefault(x => Regex.IsMatch(x, @"comments" + releaseNum + @"Ling\.txt"));

            // create 2 collections
            ObservableCollection<DataGridObject> ListOfCodeWords = new ObservableCollection<DataGridObject>();
            ObservableCollection<DataGridObject> ListOfCommentWords = new ObservableCollection<DataGridObject>();

            // reading files and populating lists

            string line;
            try
            {
                using (StreamReader f = new StreamReader(codeFile))
                {
                    while ((line = f.ReadLine()) != null)
                    {
                        string[] data = line.Split(null);
                        ListOfCodeWords.Add(new DataGridObject(data[0], int.Parse(data[1]), Math.Round(Decimal.Parse(data[2]), 6)));
                    }
                }

                using (StreamReader f = new StreamReader(commentFile))
                {
                    while ((line = f.ReadLine()) != null)
                    {
                        string[] data = line.Split(null);
                        ListOfCommentWords.Add(new DataGridObject(data[0], int.Parse(data[1]), Math.Round(Decimal.Parse(data[2]), 6)));
                    }
                }
            }

            catch
            {
                throw;
            }

            // output
            OutputCode.ItemsSource = ListOfCodeWords;
            OutputComments.ItemsSource = ListOfCommentWords;

            // fill up the whole space
            OutputCode.Columns[0].Width = new System.Windows.Controls.DataGridLength(1, System.Windows.Controls.DataGridLengthUnitType.Star);
            OutputComments.Columns[0].Width = new System.Windows.Controls.DataGridLength(1, System.Windows.Controls.DataGridLengthUnitType.Star);
        }

        private void OutputWordUtility_click(object sender, RoutedEventArgs e)
        {
            PlotWord.IsEnabled = true;
            try
            {
                OutputWordUtility();
                PopulateDropDownBox();
            }
            catch
            {
                //ShowMessageBox("The files were not found. Try calculating first.");
                return;
            } 
        }

        private void PlotWordAdditionalOptions_click(object sender, RoutedEventArgs e)
        {
            string[] file = null;
            try
            {
                string target = "";
                if (IsLingChangeRadio)          target = "CommitersAndLingChange.txt";
                else if (IsWordBirthDeathRadio) target = "BirthDeathRates.txt";
                file = File.ReadAllLines($@"{SelectedProject}/{target}");
            }
            catch
            {
                //ShowMessageBox("The files were not found. Try calculating first.");
                return;
            }
            var l1 = new List<string>();
            var l2 = new List<string>();
            foreach (var line in file)
            {
                if (!line.Equals("WordBirth,WordDeath" + Environment.NewLine))
                {
                    l1.Add(line.Split(',')[0]);
                    l2.Add(line.Split(',')[1]);
                }   
            }

            // checking for error-input
            if (l1.Count == 0 && l2.Count == 0)
            {
                //ShowMessageBox("No values were found. Perhaps the txt file is corrupted");
                return;
            }

            CreateLineSeriesForRange(l1, "Birth");
            CreateLineSeriesForRange(l2, "Death");

            PlotLineSeries();
            PlotAdditionalOptions.IsEnabled = false;
        }

        private void Releases_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string selected = comboBox.SelectedItem as string;

            // append new text
            OutputWordUtility(int.Parse(Regex.Match(selected, @"\d+").Value));
        }

        private void ClearPlot_click(object sender, RoutedEventArgs e)
        {
            ClearPlot();
            PlotWord.IsEnabled = true;
            PlotAdditionalOptions.IsEnabled = true;
        }

        private void PlotWord_click(object sender, RoutedEventArgs e)
        {
            
            var l1 = ExtractSelectedWords(OutputCode.SelectedItems);
            var l2 = ExtractSelectedWords(OutputComments.SelectedItems);

            // checking for error-input
            if (l1.Count == 0 && l2.Count == 0)
            {
                //ShowMessageBox("Please select at least one word");
                return;
            }
            else if (l1.Count != 0 && l2.Count != 0)
            {
                //ShowMessageBox("You cannot have both code and comment words selected at the same time");
                return;
            }

            List<string> l3 = null;
            if (l1.Count != 0) l3 = l1;
            else if (l2.Count != 0) l3 = l2;

            foreach (var elem in l3)
            {
                CreateLineSeriesForWord(elem);
            }

            PlotLineSeries();
            PlotWord.IsEnabled = false;
        }

        private void CancelSelection_click(object sender, RoutedEventArgs e)
        {
            OutputCode.SelectedItem = null;
            OutputComments.SelectedItem = null;
        }

        private void ProcessWholeFolder_click(object sender, RoutedEventArgs e)
        {
            LinguisticChangeMultipleWindow m = new LinguisticChangeMultipleWindow();
            m.Show();
            Close();
        }

        private void SpecifyProjectPathAdditionalOptions_click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;
                ProjectPath.Foreground = Brushes.Black;
                ProjectPath.IsEnabled = true;
                ProjectPathAdditionalOptions.Text = folderBrowserDialog.SelectedPath;

                CalculateAdditionalOptions.IsEnabled = true;
            }
        }

        private async void CalculateAdditionalOptions_click(object sender, RoutedEventArgs e)
        {
            // check which one is selected
            if (IsLingChangeRadio)
                await CalculateLinguisticChange();
            else if (IsWordBirthDeathRadio)
                CalculateBirthDeath();
        }

        private void LingChangeRadio_checked(object sender, RoutedEventArgs e)
        {
            IsLingChangeRadio = true;
            //UI.LinguisticChangeSingleWindow._LinguisticChangeSingleWindow.UniqueCommiters.IsEnabled = true;
        }

        private void LingChangeRadio_unchecked(object sender, RoutedEventArgs e)
        {
            IsLingChangeRadio = false;
            UniqueCommiters.IsEnabled = false;
        }

        private void WordBirthDeathRadio_checked(object sender, RoutedEventArgs e)
        {
            IsWordBirthDeathRadio = true;
            UniqueCommiters.IsEnabled = false;
        }

        private void WordBirthDeathRadio_unchecked(object sender, RoutedEventArgs e)
        {
            IsWordBirthDeathRadio = false;
            UniqueCommiters.IsEnabled = true;
        }

        private void UniqueCommiters_checked(object sender, RoutedEventArgs e)
        {
            IsUniqueCommiters = true;
        }

        private void UniqueCommiters_unchecked(object sender, RoutedEventArgs e)
        {
            IsUniqueCommiters = false;
        }

        private void BetweenFirstAndLast_checked(object sender, RoutedEventArgs e)
        {
            IsBetweenFirstAndLast = true;
        }

        private void BetweenFirstAndLast_unchecked(object sender, RoutedEventArgs e)
        {
            IsBetweenFirstAndLast = false;
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

        /// <summary>
        /// Opens Projects Downloader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectsDownloader_click(object sender, RoutedEventArgs e)
        {
            var input = Util.HelperFunctions.ShowChangeWindowBox();
            if (input == System.Windows.Forms.DialogResult.Yes)
            {
                ProjectsDownloaderWindow m = new ProjectsDownloaderWindow();
                m.Show();
                this.Close();
            }
        }
        #endregion

        private void CalculateBirthDeath_Click(object sender, RoutedEventArgs e)
        {
            CalculateBirthDeath();
        }

        private void ProjectInputButton2_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;
                ProjectPath.Foreground = Brushes.Black;
                ProjectPath.IsEnabled = true;
                ProjectPathAdditionalOptions.Text = folderBrowserDialog.SelectedPath;

                CalculateAdditionalOptions.IsEnabled = true;
                PlotAdditionalOptions.IsEnabled = true;
            }
        }
        /// <summary>
        /// Represents a word in a datagrid-friendly format
        /// </summary>
        private struct DataGridObject
        {
            public string Word { get; }
            public int Frequency { get; }
            public decimal Utility { get; }

            public DataGridObject(string word, int frequency, decimal utility)
            {
                Word = word;
                Frequency = frequency;
                Utility = utility;
            }
        }



        public async void CalculateBirthDeath()
        {
            SelectedProject = ProjectPathAdditionalOptions.Text;
            IsEnabled = false;
            // calculate ling change for this project
            var lc = new LinguisticChangeCalculator();
            await Task.Run(() => lc.WriteWordBirthAndDeathRatesForAProjectToFile(SelectedProject));
            LingSingleW.IsEnabled = true;
        }

        public void PopulateDropDownBox()
        {
            var tmp = Directory.GetDirectories(SelectedProject + @"\src\");
            List<string> releases = new List<string>();
            foreach (var dir in tmp)
            {
                releases.Add(new DirectoryInfo(dir).Name);
            }

            Releases_cb.ItemsSource = releases.OrderBy(x => int.Parse(Regex.Match(x, @"\d+").Value));
            Releases_cb.SelectedValue = Releases_cb.Items[0];
        }

        public void CreateLineSeriesForWord(string word)
        {
            var series = new LineSeries { Title = word };
            series.Points.AddRange(InitializeDataPointsForWord(word));
            _LineSeries.Add(series);
        }

        public void CreateLineSeriesForRange(List<string> l, string name)
        {
            var series = new LineSeries { Title = name };
            List<DataPoint> data = new List<DataPoint>();
            for (int i = 1; i < l.Count; i++)
            {
                series.Points.Add(new DataPoint(i, Double.Parse(l[i])));
            }
            _LineSeries.Add(series);
        }

        public void PlotLineSeries()
        {
            var tmp = new PlotModel { };
            foreach (var ser in _LineSeries)
            {
                tmp.Series.Add(ser);
            }
            SetUpGraph(ref tmp);
            Model = tmp;
        }

        public void ClearPlot()
        {
            _LineSeries.Clear();
            Model = null;
        }

        public List<string> ExtractSelectedWords(IList listOfSelected)
        {
            List<string> selelectedWords = new List<string>();
            foreach (object word in listOfSelected)
            {
                var t = (DataGridObject)word;
                selelectedWords.Add(t.Word);
            }
            return selelectedWords;
        }

        private List<DataPoint> InitializeDataPointsForWord(string word, string mode = "code")
        {
            List<DataPoint> ret = new List<DataPoint>();
            // get the utility values for word
            if (mode == "code")
            {
                var files = GetCodeUtilityFiles(SelectedProject);
                foreach (var file in files)
                {
                    // read file
                    using (StreamReader stream = new StreamReader(file))
                    {
                        string content = stream.ReadToEnd();
                        // find word's utility
                        Match m = Regex.Match(content, $@"^{word} \d+ (\d+\.\d+)", RegexOptions.Multiline);
                        // create a datapoint
                        var x1 = Regex.Split(Path.GetFileName(file), @"\D+")[1];
                        var y1 = m.Groups[1].Value;
                        if (y1 == "")
                            y1 = "0";
                        ret.Add(new DataPoint(Double.Parse(x1), Double.Parse(y1)));
                    }
                }
            }

            // return ordered list by release numbers
            return ret.OrderBy(x => x.X).ToList();
        }

        private void SetUpGraph(ref PlotModel mod)
        {
            var xAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                MajorStep = 1,
                //MinorTickSize = 0,
                Title = "Releases"
            };

            var yAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0.05,
                MaximumPadding = 0.05
                //MajorStep = 0.005,
            };

            mod.Axes.Add(xAxis);
            mod.Axes.Add(yAxis);
        }

        private List<string> GetCodeUtilityFiles(string pathToProject)
        {
            string[] allFiles = Directory.GetFiles(pathToProject, "*.txt");
            // check if there are ling. chage data
            List<string> ret = new List<string>();
            foreach (var file in allFiles)
            {
                if (Regex.IsMatch(file, @"code\d+Ling\.txt", RegexOptions.Compiled))
                {
                    ret.Add(file);
                }
            }
            return ret;
        }

        private List<string> GetCommentUtilityFiles(string pathToProject)
        {
            string[] allFiles = Directory.GetFiles(pathToProject, "*.txt");
            // check if there are ling. chage data
            List<string> ret = new List<string>();
            foreach (var file in allFiles)
            {
                if (Regex.IsMatch(file, @"comments\d+Ling\.txt", RegexOptions.Compiled))
                {
                    ret.Add(file);
                }
            }
            return ret;
        }

        public async Task CalculateLinguisticChange()
        {
            // STEP 1. CALCULATE AVERAGE LING. CHANGE
            SelectedProject = ProjectPathAdditionalOptions.Text;
            LingSingleW.IsEnabled = false;
            // extract release numbers with non-empty folders
            var releaseNums = Directory.GetDirectories(SelectedProject + @"\src\")
                .Where(x => (Directory.GetFiles(x).Count() != 0))
                .Select(x => int.Parse(Regex.Match(new DirectoryInfo(x).Name, @"\d+").Value)).ToList();
            // list to hold ling. change values with corresponding release numbers
            var lingChangeWithReleases = new List<Tuple<int, (decimal, decimal)>>();
            var lc = new LinguisticChangeCalculator();
            // if only first and last selected
            if (IsBetweenFirstAndLast)
            {
                var res = lc.LinguisticChangeBetweenTwoReleases(SelectedProject, releaseNums.Min(), releaseNums.Max());
                LingSingleW.IsEnabled = true;
                return;
            }
            for (int i = releaseNums.Min(); i < releaseNums.Max(); i++)
            {
                lingChangeWithReleases.Add(new Tuple<int, (decimal, decimal)>(i, lc.LinguisticChangeBetweenTwoReleases(SelectedProject, i, i + 1)));
            }

            // if unique commiters was selected - collect them
            var uniqueCommitersList = new List<Tuple<Release, List<int>>>();
            if (IsUniqueCommiters)
            {
                // STEP 2. CALCULATE UNIQUE COMMITERS
                var p = new ProjectContributorsRetrieval(SelectedProject);
                await p.Run();
                uniqueCommitersList = p.GroupCommitsByReleasesUnique();
            }

            // STEP 3. MERGE AND WRITE TO FILE
            var _path = SelectedProject + @"\CommitersAndLingChange.txt";
            using (var dataSet = new StreamWriter(_path))
            {
                try
                {
                    if (IsUniqueCommiters)
                    {
                        dataSet.Write($@"releaseNum,lingChange,devsAll,devsLeft,devsNew,devsDelta");
                    }
                    else
                    {
                        dataSet.Write($@"releasenum,lingChange");
                    }

                    dataSet.Write(Environment.NewLine);
                    for (int i = 0; i < lingChangeWithReleases.Count; i++)
                    {
                        dataSet.Write($@"{i + 1},");
                        // Ling change
                        decimal lingCh;
                        try
                        {
                            var elem = lingChangeWithReleases.Find(x => x.Item1 == i + 1);
                            //lingCh = elem.Item2;
                        }
                        catch
                        {
                            lingCh = 0;
                        }
                        //dataSet.Write($@"{lingCh}");
                        if (IsUniqueCommiters)
                        {
                            // total # of developers for that release
                            dataSet.Write($@",{uniqueCommitersList[i].Item2.Count},");
                            // # of devs who left compared to (i-1) release
                            int devsLeft = 0;
                            if (i == 0)
                            {
                                dataSet.Write($@"0,");
                            }
                            else
                            {
                                devsLeft = uniqueCommitersList[i - 1].Item2.Except(uniqueCommitersList[i].Item2).Count();
                                dataSet.Write($@"{devsLeft},");
                            }
                            // # of devs who joined compared to (i-1) release
                            int devsNew = 0;
                            if (i == 0)
                            {
                                devsNew = uniqueCommitersList[i].Item2.Count;
                                dataSet.Write($@"{devsNew},");
                            }
                            else
                            {
                                devsNew = uniqueCommitersList[i].Item2.Except(uniqueCommitersList[i - 1].Item2).Count();
                                dataSet.Write($@"{devsNew},");
                            }
                            // sum of previous two
                            dataSet.Write($@"{devsLeft + devsNew}");
                        }

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
                LingSingleW.IsEnabled = true;
                StatusBlock.Text = $"The results are saved in: {_path}";
            }

        }

        private void CodeCodeComments_checked(object sender, RoutedEventArgs e) => Mode = true;
        private void Words_checked(object sender, RoutedEventArgs e) => Mode = false;
    }
}
