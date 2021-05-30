using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

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
