using Microsoft.Win32;
using Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata.Images;
using Model.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DreamTeam.Windows.Modals
{
    /// <summary>
    /// Interaction logic for MakeProfilePost.xaml
    /// </summary>
    public partial class MakeProfilePost : Window
    {
        OpenFileDialog fileDialog;

        public MakeProfilePost()
        {
            InitializeComponent();

            fileDialog = new OpenFileDialog();
            UploadButton.IsEnabled = false;
            fileDialog.DefaultExt = ".png"; // Required file extension 
            fileDialog.Filter = "(*.png, *.mp4)|*.PNG;*.MP4"; // Optional file extensions

            fileDialog.FileOk += FileDialog_FileOk;
        }

        private void FileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UploadButton.IsEnabled = true;
            Preview.Source = new Uri(fileDialog.FileName, UriKind.Absolute);
        }

        private void MediaButton_Click(object sender, RoutedEventArgs e)
        {
            fileDialog.ShowDialog();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            int postID = ProfileQuerys.MakeProfilePost(StaticProfile.UserProfile.Id, Beschrijving.Text);

            var extension = fileDialog.FileName.Substring(fileDialog.FileName.Length - 3).ToLower();

            if(extension.Equals("png"))
            {
                Model.ProfielEnProfieldata.Images.Image img = new Model.ProfielEnProfieldata.Images.Image(fileDialog.FileName);
                img.UploadAccountPicture(postID);
            } else if(extension.Equals("mp4"))
            {
                Video vid = new Video(fileDialog.FileName);
                vid.UploadAccountVideo(postID);
            }

            PageRegistry.profileScreen.UpdateContent();

            this.Hide();
        }
    }
}
