using Model.ProfielEnProfieldata.Images;
using NUnit.Framework;
using System;
using System.IO;

namespace TestNUnit
{
    [TestFixture]
    class Data_Storage_Tests
    {
        [Test]
        public void DataFolder_Exists()
        {
            Media.TryCreateDataFolder();

            string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DreamTeam");
            Assert.IsTrue(System.IO.Directory.Exists(FolderPath));
        }
    }
}
