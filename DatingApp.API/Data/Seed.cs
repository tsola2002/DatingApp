using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        //
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;
        }

        // we making the class stic because we dont want to create a new instance of the seed class
        public static void SeedUsers(DataContext context)
        {
            // we first check our database to see if we have any users inside there
            // we only run the seed method if we have a clean database with no users in it 
            if (!context.Users.Any())
            {
                // we need to read from our UserSeedData.json
                // so we store it in a variable
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

                // we now take the data inside UserSeedData.json then convert it to user objects
                // that we can then loop through
                // we deserialize the objects and give it a type of list of user objects
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                // now that we have the user objects we can now loop through them
                foreach (var user in users)
                {
                    // for each record we need to create a user with both password & hashed password

                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();

                    // we then add the new object entries to our user
                    context.Users.Add(user);
                }

                // we then save the changes to the database
                context.SaveChanges();

            }

        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}