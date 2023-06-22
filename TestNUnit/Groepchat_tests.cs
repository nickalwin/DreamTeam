using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Model;
using Model.GroepsChat;
using Model.Matchmaking;
using Model.Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using NUnit.Framework;

namespace TestNUnit
{
    [TestFixture]
    class Groepchat_Tests
    {
        [Test]
        public void ctorgroupchat()
        {
            GroupChat groupChat = new GroupChat(1, "name", "description", 1, new DateTime(2000, 1, 1));
            Assert.IsTrue(groupChat.ToString().Equals("ID: 1, GroupName: name, Description: description, AdminID: 1, Created_at: 01-Jan-00 12:00:00 AM"));
        }

        [Test]
        public void acreateGroep()
        {
            List<int> users = new List<int>();
            Assert.IsFalse(GroepsChatQuerys.CreateGroepChat(53, "UnitTests", "test", users));
            users.Add(51);
            users.Add(57);
            users.Add(71);
            Assert.IsTrue(GroepsChatQuerys.CreateGroepChat(53, "UnitTests", "test", users));

        }

        [Test]
        public void addRemoveMembers()
        {
            List<int> users = new List<int>();
            users.Add(53);
            users.Add(57);
            users.Add(71);
            Assert.IsTrue(GroepsChatQuerys.CreateGroepChat(51, "interesting", "test", users));

            int GroupId = GroepsChatQuerys.GetIDGroupChat(51, "interesting", "test");
            foreach (int user in users)
            {
                Assert.IsTrue(GroepsChatQuerys.RemoveMember(user, GroupId));
            }
            Assert.IsTrue(GroepsChatQuerys.RemoveMember(51, GroupId));

            Assert.IsTrue(GroepsChatQuerys.RemoveMember(51, GroepsChatQuerys.GetIDGroupChat(53, "UnitTests", "test")));
            Assert.IsTrue(GroepsChatQuerys.AddMember(GroepsChatQuerys.GetIDGroupChat(53, "UnitTests", "test"), 51));
            Assert.IsTrue(GroepsChatQuerys.RemoveMember(53, GroepsChatQuerys.GetIDGroupChat(53, "UnitTests", "test")));
            Assert.IsTrue(GroepsChatQuerys.AddMember(GroepsChatQuerys.GetIDGroupChat(53, "UnitTests", "test"), 53));
            


        }

        [Test]
        public void bchangeGroepInfo()
        {
            Assert.IsTrue(GroepsChatQuerys.ChangeBeheerder(53, GroepsChatQuerys.GetIDGroupChat(51, "UnitTests", "test")));
            Assert.IsFalse(GroepsChatQuerys.ChangeBeheerder(53, GroepsChatQuerys.GetIDGroupChat(51, "UnitTests", "test")));

            Assert.IsTrue(GroepsChatQuerys.ChangeBeschrijving("changing it", GroepsChatQuerys.GetIDGroupChat(53, "UnitTests", "test")));
            Assert.IsFalse(GroepsChatQuerys.ChangeBeschrijving("changing it", GroepsChatQuerys.GetIDGroupChat(53, "UnitTests", "test")));
            Assert.IsTrue(GroepsChatQuerys.ChangeBeschrijving("UnitTests", GroepsChatQuerys.GetIDGroupChat(53, "changing it", "test")));
            
            Assert.IsTrue(GroepsChatQuerys.ChangeGroepNaam("testendanmaar", GroepsChatQuerys.GetIDGroupChat(53, "changing it", "test")));
            Assert.IsTrue(GroepsChatQuerys.ChangeGroepNaam("test", GroepsChatQuerys.GetIDGroupChat(53, "changing it", "testendanmaar")));

        }
        [Test]
        public void cdeleteGroup()
        {
            Assert.IsTrue(GroepsChatQuerys.DeleteGroepsChat(GroepsChatQuerys.GetIDGroupChat(53, "UnitTests", "test")));
            Assert.IsFalse(GroepsChatQuerys.DeleteGroepsChat(GroepsChatQuerys.GetIDGroupChat(53, "UnitTests", "test")));

        }

    }
}
