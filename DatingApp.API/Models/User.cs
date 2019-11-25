namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        //this type is a byte array 
        public byte[] PasswordHash { get; set; }

        //this is key used to recreate the hash & compare it to wat the user types in
        public byte[] PasswordSalt { get; set; }
    }
}  