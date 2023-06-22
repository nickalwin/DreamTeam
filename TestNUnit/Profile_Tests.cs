using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using NUnit.Framework;

namespace TestNUnit
{
    [TestFixture]
    class Profile_Tests
    {
        Guid UUID = Guid.NewGuid();

        [Test]
        public void A_Profile_GetSet()
        {
            bool registered = Profile.RegisterUser("TesterAcc-"+UUID.ToString(), "password-nonencrypt", $"naimverboom{UUID.ToString()}@gmail.com", "31-Jan-03", Gender.Agender);

            Assert.IsTrue(registered);
        }

        [Test]
        public void Profile_Language()
        {
            Profile profile = new Profile(-100);
            profile.Languages = new List<Language>();

            profile.AddLanguage(Language.Abkhazian);

            Assert.IsTrue(profile.Languages.Contains(Language.Abkhazian));

            profile.RemoveLanguage(Language.Abkhazian);

            Assert.IsFalse(profile.Languages.Contains(Language.Abkhazian));
        }

        [Test]
        public void Profile_Nationality()
        {
            Profile profile = new Profile(-100);
            profile.Nationalities = new List<Nationality>();

            profile.AddNationality(Nationality.Afghan);

            Assert.IsTrue(profile.Nationalities.Contains(Nationality.Afghan));

            profile.RemoveNationality(Nationality.Afghan);

            Assert.IsFalse(profile.Nationalities.Contains(Nationality.Afghan));
        }

        [Test]
        public void Update_Works()
        {
            Profile.LoginUser($"naimverboom{UUID.ToString()}@gmail.com", "password-nonencrypt");
            ProfileQuerys.ProfileAddNationality(StaticProfile.UserProfile.Id, 12);

            Assert.IsTrue(ProfileQuerys.ProfielUpdateEmail(StaticProfile.UserProfile.Id, $"nieuw-email-{UUID.ToString()}@gmail.com"));
            Assert.IsTrue(ProfileQuerys.ProfielUpdateNationality(StaticProfile.UserProfile.Id, 60));
            Assert.IsTrue(ProfileQuerys.ProfileUpdateDisplayNaam(StaticProfile.UserProfile.Id, $"Displaynaampie-{UUID.ToString()}"));
            Assert.IsTrue(ProfileQuerys.ProfileUpdateOmschrijving(StaticProfile.UserProfile.Id, $"Omschrijving-{UUID.ToString()}"));
            Assert.IsTrue(ProfileQuerys.ProfileUpdatePassword(StaticProfile.UserProfile.Id, "passwooord"));
            Assert.IsTrue(ProfileQuerys.ProfileUpdateToxicityLevel(StaticProfile.UserProfile.Id, 2.7f));
        }

        [Test]
        public void Z_RemoveAccount()
        {
            Assert.IsTrue(ProfileQuerys.DeleteProfile(ProfileQuerys.ProfileGetId($"nieuw-email-{UUID.ToString()}@gmail.com")));
        }
    }
}
