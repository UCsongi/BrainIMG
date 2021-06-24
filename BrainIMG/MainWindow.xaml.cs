using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using BrainIMG;

namespace BrainIMG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Variables
        private int recordID = -1;
        private string folderPath = string.Empty;
        #endregion Variables
        #region Properties
        /// <summary>
        /// The ID of the test record
        /// </summary>
        public int RecordID
        {
            get => recordID;

            set
            {
                recordID = value;
                OnPropertyChanged(nameof(RecordID));
            }
        }

        /// <summary>
        /// The ID of the test record
        /// </summary>
        public string FolderPath
        {
            get => folderPath;

            set
            {
                folderPath = value;
                OnPropertyChanged(nameof(FolderPath));
            }
        }

        #endregion Properties

        #region Constructor
        /// <summary>
        /// Initializes the window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Imports test data from a CSV File
        /// </summary>
        /// <param name="filePath">the path of the CSV file</param>
        /// <returns>true if successful</returns>
        private bool CsvImport()
        {
            bool result = false;
            using (var context = new BrainVisualEntities())
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = FolderPath;
                dlg.Filter = "CSV Files (*.csv)|*.csv";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFileName = dlg.FileName;
                    
                    if (File.Exists(selectedFileName))
                    {
                        try
                        {
                            StreamReader reader = new StreamReader(File.OpenRead(selectedFileName));
                            TestRecord ImportedRecord = new TestRecord() { TestDate = System.DateTime.Now };
                            ImportedRecord.FolderPath = selectedFileName;
                            ImportedRecord.TestResults = new List<TestResult>();
                            context.TestRecords.Add(ImportedRecord);
                            context.SaveChanges();

                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(',');

                                TestResult ImportedResult = new TestResult();
                                ImportedResult.RecordID = ImportedRecord.ID;

                                long.TryParse(values[0], out long temp);
                                ImportedResult.ID = temp;

                                int.TryParse(values[1], out int temp1);
                                ImportedResult.AlgoID = temp1;

                                int.TryParse(values[2], out int temp2);
                                ImportedResult.PatientID = temp2;

                                int.TryParse(values[3], out int temp3);
                                ImportedResult.LearningSize = temp3;

                                ImportedResult.AlgoParam = values[4];

                                int.TryParse(values[5], out int temp5);
                                ImportedResult.FeatureCount = temp5;

                                int.TryParse(values[6], out int temp6);
                                ImportedResult.LearnDuration = temp6;

                                int.TryParse(values[7], out int temp7);
                                ImportedResult.TestDuration = temp7;

                                int.TryParse(values[8], out int temp8);
                                ImportedResult.TN = temp8;

                                int.TryParse(values[9], out int temp9);
                                ImportedResult.FP = temp9;

                                int.TryParse(values[10], out int temp10);
                                ImportedResult.FN = temp10;

                                int.TryParse(values[11], out int temp11);
                                ImportedResult.TP = temp11;

                                float.TryParse(values[12], out float temp12);
                                ImportedResult.Stat0 = temp12;

                                float.TryParse(values[13], out float temp13);
                                ImportedResult.Stat1 = temp13;

                                float.TryParse(values[14], out float temp14);
                                ImportedResult.Stat2 = temp14;

                                float.TryParse(values[15], out float temp15);
                                ImportedResult.Stat3 = temp15;

                                float.TryParse(values[16], out float temp16);
                                ImportedResult.DiceScore = temp16;

                                float.TryParse(values[17], out float temp17);
                                ImportedResult.Stat4 = temp17;

                                float.TryParse(values[18], out float temp18);
                                ImportedResult.Stat5 = temp18;

                                float.TryParse(values[19], out float temp19);
                                ImportedResult.Stat6 = temp19;

                                float.TryParse(values[20], out float temp20);
                                ImportedResult.Stat7 = temp20;

                                context.TestResults.Add(ImportedResult);
                            }

                            context.SaveChanges();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            return result;
        }
        #region EventHandlers

        public void ImportButtonClick(object sender, RoutedEventArgs e)
        {
            if(CsvImport())
                System.Windows.MessageBox.Show("Successful Import!");
            else
                System.Windows.MessageBox.Show("Unsuccessful Import!");

        }

        public void FolderBrowseButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPath = dlg.SelectedPath;
            }

        }

        public void DisplayButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayWindow displayWindow = new DisplayWindow(FolderPath);
            displayWindow.Show();

        }


        /// <summary>
        /// Invokes the property changed event
        /// </summary>
        /// <param name="name">the name of the sender calling it</param>
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion EventHandlers
        #endregion Methods

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events
    };
}
