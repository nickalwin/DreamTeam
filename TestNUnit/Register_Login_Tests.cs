using Model;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using NUnit.Framework;
using System;

namespace TestNUnit
{
    [TestFixture]
    class Register_Login_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void usernameExists()
        {
            Assert.IsTrue(ProfileQuerys.UsernameExists("nickalwin"));
            Assert.IsFalse(ProfileQuerys.UsernameExists("fouttetest"));
        }
        [Test]
        public void EmailExistsValid()
        {
            ProfileQuerys.DeleteProfile(ProfileQuerys.ProfileGetId("onbekende@gmail.com"));
            Assert.IsTrue(ProfileQuerys.EmailExists("nickalwin@gmail.com"));
            Assert.IsFalse(ProfileQuerys.EmailExists("fouttetest@gmail.com"));
            Assert.IsTrue(!ProfileQuerys.IsValidEmail("bestaatniet@gmail.com"));
            Assert.IsTrue(ProfileQuerys.IsValidEmail("fouttetest@gmailom"));
        }
        [Test]
        public void Passwordvalid()
        {
            Assert.IsFalse(ProfileQuerys.PasswordCorrect("nick"));
            Assert.IsTrue(ProfileQuerys.PasswordCorrect("fouttetest"));
        }
        [Test]
        public void PasswordCorrect()
        {
            Assert.IsTrue(ProfileQuerys.checkPassword("nickalwin@gmail.com", "nickalwin"));
            Assert.IsFalse(ProfileQuerys.checkPassword("nickalwin@gmail.com", "fouttetest"));
        }
        [Test]
        public void RegisterUser()
        {
            ProfileQuerys.DeleteProfile(ProfileQuerys.ProfileGetId("onbekende@gmail.com"));
            Assert.IsTrue(Profile.RegisterUser("bestaatniet", "langerdanvier", "onbekende@gmail.com", "31-Jan-03", (Gender)Enum.Parse(typeof(Gender), "Man")));
            Assert.IsTrue(ProfileQuerys.DeleteProfile(ProfileQuerys.ProfileGetId("onbekende@gmail.com")));
            Assert.IsFalse(Profile.RegisterUser("nickalwin", "langerdanvier", "onbekende@gmail.com", "31-Jan-03", Gender.Man));
            Assert.IsFalse(Profile.RegisterUser("bestaatniet", "kort", "onbekende@gmail.com", "31-Jan-03", Gender.Man));
            Assert.IsFalse(Profile.RegisterUser("bestaatniet", "langerdanvier", "onbekende@gmail.c", "31-Jan-03", Gender.Man));
            Assert.IsFalse(Profile.RegisterUser("bestaatniet", "langerdanvier", "onbekende@gmail.com", "31-Jan-22", Gender.Man));
        }

        [Test]
        public void getErrorID()
        {
            int ErrorID = Profile.RegisterError(false, false, false, false, false, false, false);
            Assert.AreEqual(0, ErrorID);
            ErrorID = Profile.RegisterError(true, true, true, true, true, true, true);
            Assert.AreEqual(127, ErrorID);
        }

        [Test]
        public void loginUser()
        {
            Assert.IsTrue(Profile.LoginUser("nickalwin@gmail.com", "nickalwin"));
            Assert.IsFalse(Profile.LoginUser("onbekende@gmail.com", "nickalwin"));
            Assert.IsFalse(Profile.LoginUser("nickalwin@gmail.com", "foutwachtwoord"));
        }

        [Test]
        public void loginError()
        {
            Assert.AreEqual(3, Profile.LoginError("onbekende@gmail.com", "nickalwin"));
            Assert.AreEqual(2, Profile.LoginError("nickalwin@gmail.com", "testtest"));
            Assert.AreEqual(0, Profile.LoginError("nickalwin@gmail.com", "nickalwin"));
        }
    }
}