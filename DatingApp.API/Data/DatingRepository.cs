using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    
    public class DatingRepository : IDatingRepository
    {
        // we then initialize the field from the parameter
        private readonly DataContext _context;

        // we add a constructor called dating repository which takes in a parameter
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        // we dont use async because the add method will save the user in memory
        // until we save our changes in the database
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        // we use the same meory technique for the delete method
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            // we go into our context and return the firstordefault that matches the id that we pass in
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            // we return the actual photo
            return photo;
        }

        // we use our include method to include our photos since they're navigation properties
        // we need to specify async since its an asynce method 
        public async Task<User> GetUser(int id)
        {
            // do a check to see that u.id = id dat were passing in
            var user = await _context.Users
                                     .Include(p => p.Photos)
                                     .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            // we use toList to execute the statement go to the database & get the list of users back
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();

            // we then return the list
            return users;
        }

        public async Task<bool> SaveAll()
        {
            // this this a check that returns true if the changes are greater than zero
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
