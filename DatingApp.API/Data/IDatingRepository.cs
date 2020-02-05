using System.Threading.Tasks;
using DatingApp.API.Models;
using System.Collections.Generic;

namespace DatingApp.API.Data
{
    public class IDatingRepository
    {
        // we use c# generics to create a method that will save us from creating two separate methods
        // adding  a user & a photo we create one method to specify a type
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
    }
}