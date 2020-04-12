using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        //injecting context for use
        public AuthRepository(DataContext context)
        {
            _context = context;          
        }
        public async Task<User> Login(string username, string password)
        {
            //we create a variable to store the user from our database
            //we use lambda expression to know which user were looking for 
            var user = await _context.Users
                                        .Include(p => p.Photos)
                                        .FirstOrDefaultAsync(x => x.Username == username);

            //if we dont find a match user then return null
            if (user == null)
                return null;

            //check password against the password in the database
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                //we loop over the byte array & compare both hashes
                for (int i = 0; i < computedHash.Length; i++)
                {
                    //if the element dont match then return false 
                    if (computedHash[i] != passwordHash[i])
                     return false;  
                }
            }
            //if the elements match then return true
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            // declare byte array variables
            byte[] passwordHash, passwordSalt;
            //we pass in reference  not variables to the method
            //because we need the references to be updated
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            //set the newly generated values to the users entity password hash & salt
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //insert user into entity object
            await _context.Users.AddAsync(user);
            //save the user into the database
            await _context.SaveChangesAsync();


            //return the user
            return user;    
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //surround our call to create new instance of the class
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //anything inside using will be disposed of wen were finished with it
                //set the password salt to randomly generated key
                passwordSalt = hmac.Key;
                //this will retrieve password as a byte array
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            //AnyAsync will compare username against other users in the database
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;
                
            return false;    
        }
    }
}