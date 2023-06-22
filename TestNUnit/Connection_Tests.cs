using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using NUnit.Framework;

namespace TestNUnit
{
    [TestFixture]
    class Connection_Tests
    {

        [Test]
        public void GetProfileConnections_AddConnection()
        {
            ConnectionQuerys.RemoveConnection(-100, 1);
            Guid uuid = Guid.NewGuid();
            Assert.IsTrue(ConnectionQuerys.CreateConn(-100, 1, $"Testing-{uuid.ToString()}", ConnectionVisibility.Shown));
            StaticProfile.Platforms = ConnectionQuerys.GetPlatforms();

            List<Connection> connections = ConnectionQuerys.getProfileConnections(-100);

            Assert.IsTrue(connections.Count > 0);
        }

        [Test]
        public void MakeConnection_CheckExistance()
        {
            // Preparations
            ConnectionQuerys.RemoveConnection(-100, 1);
            Guid uuid = Guid.NewGuid();
            StaticProfile.Platforms = ConnectionQuerys.GetPlatforms();


            Assert.IsTrue(ConnectionQuerys.CreateConn(-100, 1, $"Testing-{uuid.ToString()}", ConnectionVisibility.Shown));
            // Is made
            Assert.IsTrue(ConnectionQuerys.CheckExistance(-100, 1));

            ConnectionQuerys.RemoveConnection(-100, 1);
            Assert.IsFalse(ConnectionQuerys.CheckExistance(-100, 1));
        }

        [Test]
        public void CheckExistance_DoesNotExist()
        {
            // Preparations
            StaticProfile.Platforms = ConnectionQuerys.GetPlatforms();

            // No made existance
            Assert.IsFalse(ConnectionQuerys.CheckExistance(-101, 1));
        }

        [Test]
        public void MakeConnection_EditDetails()
        {
            // Preparations
            ConnectionQuerys.RemoveConnection(-100, 1);
            Guid uuid = Guid.NewGuid();
            StaticProfile.Platforms = ConnectionQuerys.GetPlatforms();

            Assert.IsTrue(ConnectionQuerys.CreateConn(-100, 1, $"Testing-{uuid.ToString()}", ConnectionVisibility.Shown));

            Connection conn = ConnectionQuerys.getProfileConnections(-100)[0];

            Assert.AreEqual(conn.Username, $"Testing-{uuid.ToString()}");
            Assert.AreEqual(conn.Visibility, ConnectionVisibility.Shown);

            Assert.IsTrue(ConnectionQuerys.ChangeIGN(-100, 1, "NieuweNaam"));
            Assert.IsTrue(ConnectionQuerys.SetVisibility(-100, 1, ConnectionVisibility.Hidden));

            conn = ConnectionQuerys.getProfileConnections(-100)[0];

            Assert.AreEqual(conn.Username, "NieuweNaam");
            Assert.AreEqual(conn.Visibility, ConnectionVisibility.Hidden);
        }

        [Test]
        public void PlatformName_Correct()
        {
            // Preparations
            ConnectionQuerys.RemoveConnection(-100, 1);
            Guid uuid = Guid.NewGuid();
            StaticProfile.Platforms = ConnectionQuerys.GetPlatforms();


            Assert.IsTrue(ConnectionQuerys.CreateConn(-100, 1, $"Testing-{uuid.ToString()}", ConnectionVisibility.Shown));
            Connection conn = ConnectionQuerys.getProfileConnections(-100)[0];

            // Is made
            Assert.AreEqual(conn.Platform, "Steam");
        }
    }
}
