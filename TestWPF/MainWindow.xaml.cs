using Microsoft.Win32;
using System.Windows;
using Model.ProfielEnProfieldata.Images;
using Image = Model.ProfielEnProfieldata.Images.Image;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog fileDialog;
        public MainWindow()
        {
            InitializeComponent();

            // Essential TBR when application launches!
            Media.TryCreateDataFolder();

            fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".png"; // Required file extension 
            fileDialog.Filter = "Image Document (.png)|*.png"; // Optional file extensions

            fileDialog.FileOk += FileDialog_FileOk;
        }

        private void FileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UploadLabel.Content = fileDialog.FileName;

            Image img = new Image(fileDialog.FileName);
            img.UploadProfielfoto();

        }

        private void FileUploadButton_Click(object sender, RoutedEventArgs e)
        {
            fileDialog.ShowDialog();
        }

        private void FileDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            Image img = Image.GetProfilePicture(42);
            DownloadLabel.Content = img.GetFileLocation();
        }

        private void ClearCache_Click(object sender, RoutedEventArgs e)
        {
            Media.ClearCache();
        }
    }
}
