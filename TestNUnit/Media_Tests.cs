using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata.Images;
using NUnit.Framework;

namespace TestNUnit
{
    [TestFixture]
    class Media_Tests
    {
        [Test]
        public void Media_FileExists_DoesNotExists()
        {
            Media.TryCreateDataFolder();

            Assert.IsFalse(Media.FileExists("NonExistentFilename.png"));
        }

        [Test]
        public void Media_FileTooBig_Exception()
        {
            Image image = new Image("C:\\Windows\\System32\\MRT.exe");

            Assert.Throws<FileTooBigException>(image.UploadProfielfoto);
        }

        [Test]
        public void Media_UploadProfilePicture()
        {
            StaticProfile.UserProfile = new Profile(1000);

            Image image = new Image("C:\\Windows\\System32\\PerceptionSimulation\\Assets\\OpenHand.png");
            image.UploadProfielfoto();
        }

        [Test]
        public void Media_UploadAccountPicture()
        {
            StaticProfile.UserProfile = new Profile(-100);

            Image image = new Image("C:\\Windows\\System32\\PerceptionSimulation\\Assets\\OpenHand.png");
            image.UploadAccountPicture(-100);
        }

        [Test]
        public void Media_UploadPersonalPicture()
        {
            StaticProfile.UserProfile = new Profile(-100);

            Image image = new Image("C:\\Windows\\System32\\PerceptionSimulation\\Assets\\OpenHand.png");
            image.UploadPersonalPicture();
        }

        [Test]
        public void Media_GetProfilePicture()
        {
            Image img = Image.GetProfilePicture(59);

            Assert.IsTrue(Media.FileExists(img.fileLocation));
        }

        [Test]
        public void Media_GetProfilePicture_IncorrectID()
        {
            Image img = Image.GetProfilePicture(2000);

            Assert.IsFalse(Media.FileExists(img.fileLocation));
        }

        [Test]
        public void Media_GetDataFolder_NotNull()
        {
            Media.TryCreateDataFolder();
            Assert.NotNull(Media.GetDataFolder());
        }

        [Test]
        public void Media_GetFileLocation()
        {
            Media.TryCreateDataFolder();
            Image image = new Image("C:\\Windows\\System32\\MRT.exe");

            Assert.NotNull(image.GetFileLocation());
        }

        [Test]
        public void Media_ClearCache_FileGonzo()
        {
            Media.TryCreateDataFolder();
            Image image = Image.GetProfilePicture(59);

            Assert.IsTrue(Media.FileExists(image.fileLocation));
            Media.ClearCache();

            Assert.IsFalse(Media.FileExists(image.fileLocation));
        }

        //[Test]
        //public void Media_UploadPersonalVideo()
        //{
        //    StaticProfile.UserProfile = new Profile(-100);

        //    Video vid = new Video("C:\\Windows\\System32\\FakeFolder\\FakeVideo.mp4");
        //    vid.UploadPersonalVideo();
        //}

        //[Test]
        //public void Media_UploadAccountVideo()
        //{
        //    StaticProfile.UserProfile = new Profile(-100);

        //    Video vid = new Video("C:\\Windows\\System32\\FakeFolder\\FakeVideo.mp4");
        //    vid.UploadAccountVideo(-100);
        //}
    }
}
