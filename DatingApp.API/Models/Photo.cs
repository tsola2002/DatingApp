using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        //we use boolean to help the user set the photo as their main photo
        public bool IsMain { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}