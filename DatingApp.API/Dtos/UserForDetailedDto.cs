using System;
using System.Collections;
using System.Collections.Generic;
using DatingApp.API.Models;

namespace DatingApp.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Gender { get; set; }
        // DateTime type will represent date of birth(you alos have to import system for it) 
        public int Age { get; set; }
        public string KnownAs { get; set; }
        // to help show long a user has been a member
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotosForDetailedDto> Photos { get; set; }
    }
}