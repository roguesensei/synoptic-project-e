using MongoDB.Driver;
using SynopticProject_Project_E.Helpers;
using SynopticProject_Project_E.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SynopticProject_Project_E.DAL
{
    /// <summary>
    /// User Data Access Layer class
    /// </summary>
    public static class UserDAL
    {
        private static readonly string collectionName = "users";
        private static readonly string privateKey = "FCATERING";

        /// <summary>
        /// Get a user by card ID
        /// </summary>
        /// <param name="cardId">Card ID</param>
        /// <returns>User object</returns>
        public static User GetUser(string cardId)
        {
            var appSettings = ConfigurationHelper.GetAppSettings();
            var client = new MongoClient(appSettings.ConnectionString);

            var user = client.GetDatabase(appSettings.DatabaseName)
                 .GetCollection<User>(collectionName)
                 .Find(x => x.CardId == EncryptionHelper.Encrypt(cardId, cardId, privateKey))
                 .FirstOrDefault();

            if (user != null)
            {
                // Decrypt information
                user = new User
                {
                    CardId = EncryptionHelper.Decrypt(user.CardId, cardId, privateKey),
                    PIN = EncryptionHelper.Decrypt(user.PIN, cardId, privateKey),
                    EmployeeId = user.EmployeeId,
                    FirstName = EncryptionHelper.Decrypt(user.FirstName, cardId, privateKey),
                    Surname = EncryptionHelper.Decrypt(user.Surname, cardId, privateKey),
                    EmailAddress = EncryptionHelper.Decrypt(user.EmailAddress, cardId, privateKey),
                    MobileNumber = EncryptionHelper.Decrypt(user.MobileNumber, cardId, privateKey),
                    IsAdmin = user.IsAdmin
                };
            }

            return user;
        }

        /// <summary>
        /// Get a list of the users
        /// </summary>
        /// <returns>List of encrypted users</returns>
        public static List<User> GetUsers()
        {
            var appSettings = ConfigurationHelper.GetAppSettings();
            var client = new MongoClient(appSettings.ConnectionString);

            var users = client.GetDatabase(appSettings.DatabaseName)
                 .GetCollection<User>(collectionName)
                 .AsQueryable()
                 .ToList();

            return users;
        }

        /// <summary>
        /// Check if a user is valid (i.e. exists)
        /// </summary>
        /// <param name="cardId">User Card ID</param>
        /// <param name="pin">User PIN</param>
        /// <returns>Valid or not</returns>
        public static bool IsValidUser(string cardId, string pin)
        {
            var user = GetUser(cardId);

            return user != null && user.PIN == pin;
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="user">User information</param>
        /// <param name="isAdmin">Whether or not the user is an admin</param>
        /// <returns>Success or failure</returns>
        public static bool CreateUser(UserUploadModel user, bool isAdmin = false)
        {
            var appSettings = ConfigurationHelper.GetAppSettings();
            var client = new MongoClient(appSettings.ConnectionString);
            string cardId = user.CardId;

            try
            {
                client.GetDatabase(appSettings.DatabaseName)
                    .GetCollection<User>(collectionName)
                    .InsertOne(new User
                    {
                        _id = Guid.NewGuid(),
                        CardId = EncryptionHelper.Encrypt(user.CardId, cardId, privateKey),
                        PIN = EncryptionHelper.Encrypt(user.PIN, cardId, privateKey),
                        EmployeeId = user.EmployeeId,
                        FirstName = EncryptionHelper.Encrypt(user.FirstName, cardId, privateKey),
                        Surname = EncryptionHelper.Encrypt(user.Surname, cardId, privateKey),
                        EmailAddress = EncryptionHelper.Encrypt(user.EmailAddress, cardId, privateKey),
                        MobileNumber = EncryptionHelper.Encrypt(user.MobileNumber, cardId, privateKey),
                        IsAdmin = isAdmin
                    });
            }
            catch (Exception exc)
            {
                return false;
            }

            return true;
        }
    }
}
