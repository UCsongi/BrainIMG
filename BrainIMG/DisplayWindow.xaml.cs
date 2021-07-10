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
        private string[] originalImagePaths;
        private string inputFolderPath = "";
        private ObservableCollection<string> images = new ObservableCollection<string>();
        private ObservableCollection<string> originalImages = new ObservableCollection<string>();
        private ObservableCollection<string> values = new ObservableCollection<string>();
        private ObservableCollection<string> properties = new ObservableCollection<string>();
        private ObservableCollection<string> inputImageTypes = new ObservableCollection<string>() { "T1", "T2", "FLAIR", "TIC", "GT" };

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
        public ObservableCollection<string> Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
                OnPropertyChanged(nameof(Values));
            }
        }
        public ObservableCollection<string> Properties
        {
            get
            {
                return properties;
            }
            set
            {
                properties = value;
                OnPropertyChanged(nameof(Properties));
            }
        }
        public ObservableCollection<string> InputImageTypes
        {
            get
            {
                return inputImageTypes;
            }
            set
            {
                inputImageTypes = value;
                OnPropertyChanged(nameof(InputImageTypes));
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

        private string selectedInputName = string.Empty;
        public string SelectedInputName
        {
            get => selectedInputName;

            set
            {
                if (selectedInputName != value)
                {
                    selectedInputName = value;
                    OnPropertyChanged(nameof(SelectedInputName));
                    using (var context = new BrainVisualEntities())
                    {
                        if (string.IsNullOrEmpty(inputFolderPath))
                        {
                            FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
                            dlg.SelectedPath = System.IO.Directory.GetParent(FolderPath)?.FullName;
                            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                inputFolderPath = dlg.SelectedPath;

                                string filename = "pat";
                                switch (value)
                                {
                                    case "T1":
                                        PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                        int.TryParse(patientID, out int temp);
                                        if (temp < 10)
                                            filename = filename + "0" + PatientID + "-ch0";
                                        else
                                            filename = filename + PatientID + "-ch0";
                                        break;
                                    case "T2":
                                        PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                        int.TryParse(patientID, out int temp1);
                                        if (temp1 < 10)
                                            filename = filename + "0" + PatientID + "-ch1";
                                        else
                                            filename = filename + PatientID + "-ch1";
                                        break;
                                    case "FLAIR":
                                        PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                        int.TryParse(patientID, out int temp2);
                                        if (temp2 < 10)
                                            filename = filename + "0" + PatientID + "-ch2";
                                        else
                                            filename = filename + PatientID + "-ch2"; break;
                                    case "TIC":
                                        PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                        int.TryParse(patientID, out int temp3);
                                        if (temp3 < 10)
                                            filename = filename + "0" + PatientID + "-ch3";
                                        else
                                            filename = filename + PatientID + "-ch3"; 
                                        break;
                                    case "GT":
                                        PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                        int.TryParse(patientID, out int temp4);
                                        if (temp4 < 10)
                                            filename = filename + "0" + PatientID + "-gt";
                                        else
                                            filename = filename + PatientID + "-gt";
                                        break;
                                    default:
                                        break;
                                }

                                DisplayImage(inputFolderPath + "\\" + filename + ".png");
                            }
                        }
                        else
                        {
                            string filename = "pat";
                            switch (value)
                            {
                                case "T1":
                                    PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                    int.TryParse(patientID, out int temp);
                                    if (temp < 10)
                                        filename = filename + "0" + PatientID + "-ch0";
                                    else
                                        filename = filename + PatientID + "-ch0";
                                    break;
                                case "T2":
                                    PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                    int.TryParse(patientID, out int temp1);
                                    if (temp1 < 10)
                                        filename = filename + "0" + PatientID + "-ch1";
                                    else
                                        filename = filename + PatientID + "-ch1";
                                    break;
                                case "FLAIR":
                                    PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                    int.TryParse(patientID, out int temp2);
                                    if (temp2 < 10)
                                        filename = filename + "0" + PatientID + "-ch2";
                                    else
                                        filename = filename + PatientID + "-ch2"; break;
                                case "TIC":
                                    PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                    int.TryParse(patientID, out int temp3);
                                    if (temp3 < 10)
                                        filename = filename + "0" + PatientID + "-ch3";
                                    else
                                        filename = filename + PatientID + "-ch3";
                                    break;
                                case "GT":
                                    PatientID = context.TestResults.FirstOrDefault(x => x.ID + ".png" == SelectedImageName).PatientID.ToString();
                                    int.TryParse(patientID, out int temp4);
                                    if (temp4 < 10)
                                        filename = filename + "0" + PatientID + "-gt";
                                    else
                                        filename = filename + PatientID + "-gt";
                                    break;
                                default:
                                    break;
                            }

                            DisplayImage(inputFolderPath + "\\" + filename + ".png");
                        }
                    }
                }
            }
        }


        private string selectedPropertyName = string.Empty;
        public string SelectedPropertyName
        {
            get => selectedPropertyName;

            set { 
                 
                
                if (selectedPropertyName != value)
                {
                    selectedPropertyName = value;
                    OnPropertyChanged(nameof(SelectedPropertyName));

                    using (var context = new BrainVisualEntities())
                    {
                        switch (value)
                        {
                            case "AlgoID":
                                List<string> temp = new List<string>();
                                Values = new ObservableCollection<string>(context.TestResults.Select(x => x.AlgoID.ToString()).Distinct().ToList());
                                break;
                            case "AlgoParam":
                                List<string> temp1 = new List<string>();
                                Values = new ObservableCollection<string>(context.TestResults.Select(x => x.AlgoParam.ToString()).Distinct().ToList());
                                break;
                            case "FeatureCount":
                                List<string> temp2 = new List<string>();
                                Values = new ObservableCollection<string>(context.TestResults.Select(x => x.FeatureCount.ToString()).Distinct().ToList());
                                break;
                            case "LearningSize":
                                List<string> temp3 = new List<string>();
                                Values = new ObservableCollection<string>(context.TestResults.Select(x => x.LearningSize.ToString()).Distinct().ToList());
                                break;
                            case "PatientID":
                                List<string> temp4 = new List<string>();
                                Values = new ObservableCollection<string>(context.TestResults.Select(x => x.PatientID.ToString()).Distinct().ToList());
                                break;
                            default:
                                break;
                        } }
                }
            }
        }


        private string selectedValueName = string.Empty;
        public string SelectedValueName
        {
            get => selectedValueName;

            set
            {
                if (selectedValueName != value)
                {
                    selectedValueName = value;
                    OnPropertyChanged(nameof(SelectedValueName));
                    using (var context = new BrainVisualEntities())
                    {
                        ObservableCollection<string> tempImages = new ObservableCollection<string>();
                        foreach(string name in originalImages)
                        {
                            switch (SelectedPropertyName)
                            {
                                case "AlgoID":
                                    if (context.TestResults.FirstOrDefault(x => x.ID + ".png" == name).AlgoID.ToString() == value)
                                        tempImages.Add(name);
                                    break;
                                case "AlgoParam":
                                    if (context.TestResults.FirstOrDefault(x => x.ID + ".png" == name).AlgoParam.ToString() == value)
                                        tempImages.Add(name); 
                                    break;
                                case "FeatureCount":
                                    if (context.TestResults.FirstOrDefault(x => x.ID + ".png" == name).FeatureCount.ToString() == value)
                                        tempImages.Add(name);
                                    break;
                                case "LearningSize":
                                    if (context.TestResults.FirstOrDefault(x => x.ID + ".png" == name).LearningSize.ToString() == value)
                                        tempImages.Add(name);
                                    break;
                                case "PatientID":
                                    if (context.TestResults.FirstOrDefault(x => x.ID + ".png" == name).PatientID.ToString() == value)
                                        tempImages.Add(name);
                                    break;
                                default:
                                    break;
                            }
                        }
                        Images = tempImages;
                        imagePaths = tempImages.Select(x => FolderPath + "\\" + x).ToArray();
                    }
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

        private string ptpr = string.Empty;
        public string PTPR
        {
            get => ptpr;

            set
            {
                ptpr = value;
                OnPropertyChanged(nameof(PTPR));
            }
        }

        private string ptnr = string.Empty;
        public string PTNR
        {
            get => ptnr;

            set
            {
                ptnr = value;
                OnPropertyChanged(nameof(PTNR));
            }
        }

        private string pppv = string.Empty;
        public string PPPV
        {
            get => pppv;

            set
            {
                pppv = value;
                OnPropertyChanged(nameof(PPPV));
            }
        }

        private string correctness = string.Empty;
        public string Correctness
        {
            get => correctness;

            set
            {
                correctness = value;
                OnPropertyChanged(nameof(Correctness));
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
                originalImages = Images = new ObservableCollection<string>(filesFound.Select(f => Path.GetFileName(f)));
                imagePaths = filesFound?.ToArray();
                originalImagePaths = filesFound?.ToArray(); 

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
                Properties = new ObservableCollection<string>()
                {
                    nameof(TestResult.AlgoID),
                    nameof(TestResult.AlgoParam),
                     nameof(TestResult.FeatureCount),
                      nameof(TestResult.LearningSize),
                       nameof(TestResult.PatientID)
                };
                TestResult temp = context.TestResults.FirstOrDefault(x => x.ID.ToString() == imageName.Substring(0, imageName.Length - 4));
                DiceScore = (temp?.DiceScore * 100).ToString();
                DiceScore = DiceScore.Length > 7 ? DiceScore.Substring(0, 7) + "%" : DiceScore;
                TN = temp?.TN.ToString();
                FP = temp?.FP.ToString();
                FN = temp?.FN.ToString();
                TP = temp?.TP.ToString();
                AlgoID = temp?.AlgoID.ToString();
                PatientID = temp?.PatientID.ToString();
                AlgoParam = temp?.AlgoParam.ToString();
                PTPR = (temp?.PTPR * 100).ToString();
                PTPR = PTPR.Length > 7 ? PTPR.Substring(0, 7) + "%" : PTPR;
                PTNR = (temp?.PTNR * 100).ToString();
                PTNR = PTNR.Length > 7 ? PTNR.Substring(0, 7) + "%" : PTNR;
                PPPV = (temp?.PPPV * 100).ToString();
                PPPV = PPPV.Length > 7 ? PPPV.Substring(0, 7) + "%" : PPPV;
                Correctness = (temp?.Correctness * 100).ToString();
                Correctness = Correctness.Length > 7 ? Correctness.Substring(0, 7) + "%" : Correctness;
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

