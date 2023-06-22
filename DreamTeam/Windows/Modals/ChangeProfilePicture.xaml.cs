using Microsoft.Win32;
using Model.ProfielEnProfieldata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DreamTeam.Windows.Modals
{
    /// <summary>
    /// Interaction logic for ChangeProfilePicture.xaml
    /// </summary>
    public partial class ChangeProfilePicture : Window
    {
        OpenFileDialog fileDialog;

        public ChangeProfilePicture()
        {
            InitializeComponent();

            fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".png"; // Required file extension 
            fileDialog.Filter = "Image Document (.png)|*.png"; // Optional file extensions

            fileDialog.FileOk += FileDialog_FileOk;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            Model.ProfielEnProfieldata.Images.Image img = new Model.ProfielEnProfieldata.Images.Image(fileDialog.FileName);
            img.UploadProfielfoto();
            Model.ProfielEnProfieldata.Images.Image.GetProfilePicture(StaticProfile.UserProfile.Id);

            PageRegistry.profileScreen.UpdateProfilePic();

            this.Hide();
        }

        private void FileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UploadLabel.Content = "Locatie: " + fileDialog.FileName;

            UploadButton.IsEnabled = true;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            fileDialog.ShowDialog();
        }
    }
}
