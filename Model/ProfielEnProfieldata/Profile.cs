using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata.Images;
using Model.SQLData;
using MySqlConnector;

namespace Model
{
    public class Profile
    {
        public List<Game> Games;
        private List<Connection> Connections;
        public string ProfileText { get; set; }
        public string Email { get; set; }
        public float ToxicityLevel { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int Age { get; set; }
        public string Nationaliteit { get; set; }
        public Gender Gender { get; set; }
        public List<Nationality> Nationalities { get; set; }
        public List<Language> Languages { get; set; }
        public string Omschrijving { get; set; }

        /// <summary>
        /// Adds a language to the profile.
        /// </summary>
        /// <param name="language"></param>
        public void AddLanguage(Language language)
        {
            Languages.Add(language);
        }

        /// <summary>
        /// Removes a language from the profile.
        /// </summary>
        /// <param name="language"></param>
        public void RemoveLanguage(Language language)
        {
            Languages.Remove(language);
        }

        /// <summary>
        /// Adds a nationality to the profile.
        /// </summary>
        /// <param name="nationality"></param>
        public void AddNationality(Nationality nationality)
        {
            Nationalities.Add(nationality);
        }

        /// <summary>
        /// Removes nationality from the profile.
        /// </summary>
        /// <param name="nationality"></param>
        public void RemoveNationality(Nationality nationality)
        {
            Nationalities.Remove(nationality);
        }

        public Profile(int id, string displayname, string email, int age, Gender gender, string profileText, float toxicityLevel, List<Language> languages, List<Game> games, List<Connection> connections, List<Nationality> nationalities, string omschrijving)
        {
            Languages = languages;
            Games = games;
            Connections = connections;
            Id = id;
            Email = email;
            DisplayName = displayname;
            Age = age;
            Gender = gender;
            Nationalities = nationalities;
            ProfileText = profileText;
            ToxicityLevel = toxicityLevel;
            Omschrijving = omschrijving;
        }

        public Profile(int id)
        {
            Id = id;
        }

        public Profile(int id, string name)
        {
            Id = id;
            DisplayName = name;
        }

        /// <summary>
        /// Registers user. Also encrypts passwords. Assumes checks have been done.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="age"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        static public bool RegisterUser(string name, string password, string email, string age, Gender gender)
        {
            DateTime age2 = Convert.ToDateTime(age);
            int actualage = DateTime.Now.Year - age2.Year;
            if (!ProfileQuerys.UsernameExists(name) && !ProfileQuerys.EmailExists(email) && !ProfileQuerys.IsValidEmail(email) && ProfileQuerys.PasswordCorrect(password) && actualage >= 16 && actualage <= 199)
            {
                byte[] salt = RandomString(16);
                byte[] saltedPass = GenerateSaltedHash(Encoding.UTF8.GetBytes(password), salt);

                return ProfileQuerys.ProfileRegister(name, Convert.ToBase64String(saltedPass), email, age, gender, Convert.ToBase64String(salt));
            }
            return false;
        }

        /// <summary>
        /// Gets correct error.
        /// </summary>
        /// <param name="nameError"></param>
        /// <param name="emailError"></param>
        /// <param name="emailValidError"></param>
        /// <param name="passwordError"></param>
        /// <param name="ageError"></param>
        /// <param name="genderError"></param>
        /// <param name="nationalityError"></param>
        /// <returns></returns>
        static public int RegisterError(bool nameError, bool emailError, bool emailValidError, bool passwordError, bool ageError, bool genderError, bool nationalityError)
        {
            int Result = 0;
            if (nameError)
            {
                Result += 64;
            }
            if (emailError)
            {
                Result += 32;
            }
            if (emailValidError)
            {
                Result += 16;
            }
            if (passwordError)
            {
                Result += 8;
            }
            if (ageError)
            {
                Result += 4;
            }
            if (genderError)
            {
                Result += 2;
            }
            if (nationalityError)
            {
                Result += 1;
            }
            return Result;
        }

        /// <summary>
        /// Logs in user, also checks password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static public bool LoginUser(string email, string password)
        {
            return ProfileQuerys.checkPassword(email, password);
        }

        /// <summary>
        /// Gets correct error.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static public int LoginError(string email, string password)
        {
            int result = 0;
            if (!ProfileQuerys.checkPassword(email, password))
            {
                result += 2;
            }
            if (!ProfileQuerys.EmailExists(email))
            {
                result += 1;
            }
            return result;
        }

        /// <summary>
        /// Generates salted hash based on hash and plaintext password.
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        /// <summary>
        /// Generates random string for hash generation.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            byte[] salt = new byte[length];
            for(int i = 0; i < length; i++)
            {
                salt[i] = Convert.ToByte(chars[random.Next(chars.Length)]);
            }

            return salt;
        }
    }
}
