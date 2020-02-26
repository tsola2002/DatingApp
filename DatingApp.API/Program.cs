using System;
using DatingApp.API.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // we store a reference to createWebHostBuilder()
            // we defer the run till later 
            var host = CreateWebHostBuilder(args).Build();

            // even though we cant inject anything into our main method
            // we still need to get an instance of our datacontext so that we can pass it to our seed users method
            // we want to dispose of our datacontext as soon as we've seeded our users
            // so we wrap everything inside of a using statement
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                // we need to catch any errors incase something goes wrong
                try
                {
                    // we first need to get our data context
                    var context = services.GetRequiredService<DataContext>();
                    // we will use the database migrate command
                    // this will run ef database update command as soon the program runs
                    context.Database.Migrate();

                    // we now seed our users
                    Seed.SeedUsers(context);
                }
                catch (Exception ex)
                {
                    // we get an Ilogger incase of any errors
                    // it needs to be of the type its logging against which is the program class
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    // we log an error once we have access to the logger
                    logger.LogError(ex, "An error occured during migration");
                }
            }

            // we run the host after the data is seeded
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
