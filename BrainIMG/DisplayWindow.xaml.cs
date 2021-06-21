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

        /// <summary>
        /// The ID of the test record
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
        #region Constructor
        /// <summary>
        /// Initializes the Window
        /// </summary>
        public DisplayWindow(string folderPath)
        {
            InitializeComponent();
            FolderPath = folderPath;
        }

        public ObservableCollection<string> Images = new ObservableCollection<string>();

        private string[] ImagePaths;
        #endregion Constructor

        #region Methods
        public string[] GetFilesFrom(string searchFolder)
        {
            List<string> filesFound = new List<string>();
            filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", "png"), SearchOption.TopDirectoryOnly));

            Images = new ObservableCollection<string>(filesFound.Select(f => Path.GetFileName(f)));
            return filesFound.ToArray();
        }
        #endregion Methods

        #region EventHandlers
        /// <summary>
        /// Opens a file explorer to open an image
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event arguments</param>
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            ImagePaths = GetFilesFrom(FolderPath);


            /*

            OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFileName = dlg.FileName;
                FileNameLabel.Content = selectedFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                ImageViewer1.Source = bitmap;
            }*/
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

