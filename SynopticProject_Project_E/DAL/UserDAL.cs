using MongoDB.Driver;
using SynopticProject_Project_E.Models;
using System;
using System.Linq;

namespace SynopticProject_Project_E.DAL
{
    public static class UserDAL
    {
        // TODO: Move to config
        private static readonly string connectionString = "mongodb://localhost";

        public static User GetUser(string cardId)
        {
            var client = new MongoClient(connectionString);

            var user = client.GetDatabase("first-catering")
                 .GetCollection<User>("users")
                 .Find(x => x.CardId == cardId)
                 .FirstOrDefault();

            return user;
        }

        public static bool CreateUser(User user)
        {
            var client = new MongoClient(connectionString);

            try
            {
                client.GetDatabase("users").GetCollection<User>("users").InsertOne(user);
            }
            catch (Exception exc)
            {
                return false;
            }

            return true;
        }
    }
}
