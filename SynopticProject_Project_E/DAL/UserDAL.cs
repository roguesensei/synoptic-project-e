using MongoDB.Driver;
using SynopticProject_Project_E.Helpers;
using SynopticProject_Project_E.Models;
using System;
using System.Linq;

namespace SynopticProject_Project_E.DAL
{
    public static class UserDAL
    {
        // TODO: Move to config
        private static readonly string connectionString = "mongodb://localhost";
        private static readonly string privateKey = "FCATERING";

        public static User GetUser(string cardId)
        {
            var client = new MongoClient(connectionString);

            var user = client.GetDatabase("first-catering")
                 .GetCollection<User>("users")
                 .Find(x => x.CardId == EncryptionHelper.Encrypt(cardId, cardId, privateKey))
                 .FirstOrDefault();

            if (user != null)
            {
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

        public static bool CreateUser(UserUploadModel user)
        {
            var client = new MongoClient(connectionString);
            string cardId = user.CardId;

            try
            {
                client.GetDatabase("first-catering")
                    .GetCollection<User>("users")
                    .InsertOne(new User
                    {
                        CardId = EncryptionHelper.Encrypt(user.CardId, cardId, privateKey),
                        PIN = EncryptionHelper.Encrypt(user.PIN, cardId, privateKey),
                        EmployeeId = user.EmployeeId,
                        FirstName = EncryptionHelper.Encrypt(user.FirstName, cardId, privateKey),
                        Surname = EncryptionHelper.Encrypt(user.Surname, cardId, privateKey),
                        EmailAddress = EncryptionHelper.Encrypt(user.EmailAddress, cardId, privateKey),
                        MobileNumber = EncryptionHelper.Encrypt(user.MobileNumber, cardId, privateKey),
                        IsAdmin = false
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
