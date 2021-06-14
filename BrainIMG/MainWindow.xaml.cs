using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
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
        private int RecordID
        {
            get => recordID;

            set
            {
                recordID = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The ID of the test record
        /// </summary>
        private string FolderPath
        {
            get => folderPath;

            set
            {
                folderPath = value;
                OnPropertyChanged();
            }
        }

        private TestRecord ImportedRecord
        { get; set; } = new TestRecord() { TestDate = System.DateTime.Now};
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Initializes the window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Methods
        #region EventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BrainVisualEntities())
            {
                string filePath = "eval.csv";
                ImportedRecord.FolderPath = filePath;
                ImportedRecord.Metadata = "First import.";
                ImportedRecord.TestResults = new List<TestResult>();
                context.TestRecords.Add(ImportedRecord);
                context.SaveChanges();
                if (File.Exists(filePath))
                {
                    StreamReader reader = new StreamReader(File.OpenRead(filePath));
                    List<string> listA = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        TestResult ImportedResult = new TestResult();
                        ImportedResult.RecordID = ImportedRecord.ID;

                        context.TestResults.Add(ImportedResult);
                        context.SaveChanges();

                        ImportedResult.Value3 = values[0];

                        int.TryParse(values[2], out int temp);
                        ImportedResult.Value2 = temp;
                        ImportedResult.Value1 = values[1];
                        context.SaveChanges();
                    }
                }
            }
        }
        

        /// <summary>
        /// Invokes the property changed event
        /// </summary>
        /// <param name="name">the name of the sender calling it</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
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
