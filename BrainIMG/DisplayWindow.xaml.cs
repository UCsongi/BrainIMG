using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace BrainIMG
{
    /// <summary>
    /// Interaction logic for DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window, INotifyPropertyChanged
    {
        int ImgIndex = 0;
        public static readonly List<string> imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
        private string[] imagePaths;
        private ObservableCollection<string> images = new ObservableCollection<string>();

        /// <summary>
        /// The folder path
        /// </summary>
        string folderPath = string.Empty;
        public string FolderPath
        {
            get => folderPath;

            set
            {
                folderPath = value;
                OnPropertyChanged(nameof(FolderPath));
            }
        }

        public ObservableCollection<string> Images
        {
            get
            {
                return images;
            }
            set
            {
                images = value;
                OnPropertyChanged(nameof(Images));
            }
        }


        private string selectedImageName = string.Empty;
        public string SelectedImageName
        {
            get => selectedImageName;

            set
            {
                if (selectedImageName != value)
                {
                    selectedImageName = value;
                    OnPropertyChanged(nameof(SelectedImageName));
                    InitializeImage(FolderPath + @"\" + SelectedImageName);
                    ImgIndex = Images.IndexOf(selectedImageName);
                }
            }
        }

        private string diceScore = string.Empty;
        public string DiceScore
        {
            get => diceScore;

            set
            {
                diceScore = value;
                OnPropertyChanged(nameof(DiceScore));
            }
        }

        private string tn = string.Empty;
        public string TN
        {
            get => tn;

            set
            {
                tn = value;
                OnPropertyChanged(nameof(TN));
            }
        }

        private string fp = string.Empty;
        public string FP
        {
            get => fp;

            set
            {
                fp = value;
                OnPropertyChanged(nameof(FP));
            }
        }

        private string tp = string.Empty;
        public string TP
        {
            get => tp;

            set
            {
                tp = value;
                OnPropertyChanged(nameof(TP));
            }
        }

        private string fn = string.Empty;
        public string FN
        {
            get => fn;

            set
            {
                fn = value;
                OnPropertyChanged(nameof(FN));
            }
        }

        private string algoID = string.Empty;
        public string AlgoID
        {
            get => algoID;

            set
            {
                algoID = value;
                OnPropertyChanged(nameof(AlgoID));
            }
        }

        private string algoParam = string.Empty;
        public string AlgoParam 
        {
            get => algoParam;

            set
            {
                algoParam = value;
                OnPropertyChanged(nameof(AlgoParam));
            }
        }

        private string patientID = string.Empty;
        public string PatientID
        {
            get => patientID;

            set
            {
                patientID = value;
                OnPropertyChanged(nameof(PatientID));
            }
        }

        private string filePath = string.Empty;
        public string FilePath
        {
            get => filePath;

            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        #region Constructor
        /// <summary>
        /// Initializes the Window
        /// </summary>
        public DisplayWindow(string folderPath)
        {
            InitializeComponent();
            this.DataContext = this;
            ChangeFolderPath(folderPath);
        }

        #endregion Constructor

        #region Methods
        public List<string> GetFilesFrom(string searchFolder)
        {
            List<string> filesFound = new List<string>();
            filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", "png"), SearchOption.TopDirectoryOnly));

            return filesFound;
        }


        public void ChangeFolderPath(string path)
        {
            FolderPath = path;
            if (!string.IsNullOrEmpty(FolderPath))
            {

                List<string> filesFound = GetFilesFrom(FolderPath);
                Images = new ObservableCollection<string>(filesFound.Select(f => Path.GetFileName(f)));
                imagePaths = filesFound?.ToArray();

                if (imagePaths.Length > 0)
                {
                    SelectedImageName = Path.GetFileName(imagePaths[0]);
                }
            }
        }

        public void InitializeImage(string path)
        {
            if (File.Exists(path) && imageExtensions.Contains(Path.GetExtension(path).ToUpperInvariant()))
            {
                if (imagePaths.Length == 0)
                    imagePaths = new string[50];

                DisplayImage(path);

                if (Images.Count > 0)
                    InitializeDisplayedData(Path.GetFileName(path));
            }
        }

        public void DisplayImage(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();
            ImageViewer1.Source = bitmap;
        }

        public void InitializeDisplayedData(string imageName)
        {
            using (var context = new BrainVisualEntities())
            {
                TestResult temp = context.TestResults.FirstOrDefault(x => x.ID.ToString() == imageName.Substring(0, imageName.Length - 4));
                DiceScore = (temp?.DiceScore * 100).ToString() + "%";
                TN = temp?.TN.ToString();
                FP = temp?.FP.ToString();
                FN = temp?.FN.ToString();
                TP = temp?.TP.ToString();
                AlgoID = temp?.AlgoID.ToString();
                PatientID = temp?.PatientID.ToString();
                AlgoParam = temp?.AlgoParam.ToString();
            }
        }
        #endregion Methods

        #region EventHandlers
        /// <summary>
        /// Opens a file explorer to open an image
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event arguments</param>
        public void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                    ChangeFolderPath(dlg.SelectedPath);
            }
        }

        public void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImgIndex + 1 < imagePaths.Length)
            {
                SelectedImageName = Images[ImgIndex + 1];
            }
            else if (ImgIndex + 1 >= imagePaths.Length)
            {
                SelectedImageName = Images[0];
            }
        }

        public void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImgIndex - 1 >= 0)
            {
                SelectedImageName = Images[ImgIndex - 1];
            }
            else if(ImgIndex - 1 < 0)
            {
                SelectedImageName = Images[Images.Count - 1];
            }
        }


        #endregion EventHandlers


        /// <summary>
        /// Invokes the property changed event
        /// </summary>
        /// <param name="name">the name of the sender calling it</param>
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events

    };
}

