using System;
using System.Collections;
using System.Collections.Generic;

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

        public string Gender { get; set; }
        //DateTime type will represent date of birth(you alos have to import system for it) 
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        // to help show long a user has been a member
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        // we add a collection of type Photo(we import systems.collections.generic for icollection)
        public ICollection<Photo> Photos { get; set; }
    }
}