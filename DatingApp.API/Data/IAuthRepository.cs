using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        //task of type user which takes in username & password
        Task<User> Register(User user, string password);
        //logs in usernames based on username & password
        Task<User> Login(string username, string password);
        //checks username to see if they exist
        Task<bool> UserExists(string username);
    } 
}
