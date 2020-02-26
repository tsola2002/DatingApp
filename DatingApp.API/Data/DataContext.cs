using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{ 
    // we inherit from a higher level class DbContext which uses an assembly reference
    //DataContext is a derived class of DbContext
    public class DataContext : DbContext
    {

        //db context must have an instance of dbcontext options
        //so we pass the options to the base constructor by chaining it to the top
        public DataContext(DbContextOptions<DataContext> options): base (options) {}


        //inorder to tell entity framework about our entities, we need to give it some properties
        //the properties will be of type dbset
        //then we will give it a name which will be used as the table display
        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }
    }
}