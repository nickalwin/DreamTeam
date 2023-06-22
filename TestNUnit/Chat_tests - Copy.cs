using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Model;
using Model.Matchmaking;
using Model.Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using NUnit.Framework;

namespace TestNUnit
{
    [TestFixture]
    class Chat_Tests
    {
        [Test]
        public void aSend_Get_DeleteMessage()
        {
            Profile newProfile = new Profile(1, "test-account");
            Profile newProfile2 = new Profile(2, "test-account");
            StaticProfile.UserProfile = newProfile;
            Assert.IsTrue(ChatQuerys.SendMessage(2, "dit is een unittest"));
            StaticProfile.UserProfile = newProfile2;
            Assert.AreEqual("dit is een unittest", ChatQuerys.GetMessages(1)[0].MessageContent);
            StaticProfile.UserProfile = newProfile;
            Assert.IsTrue(ChatQuerys.DeleteMessages(2));
            Assert.IsFalse(ChatQuerys.DeleteMessages(2));
        }
    }
}
