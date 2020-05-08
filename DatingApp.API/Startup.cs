using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

    
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //it allows us to create services which can be injected into our application
        //it is also called the dependency injection container
        public void ConfigureServices(IServiceCollection services)
        {
            //add DbContext & pass in our type which is datacontext
            //we pass in an expression for the dbcontext on which database driver to use
            //we specify the connection string to use
            //we add cors support as a service
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = 
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddCors();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper(typeof(DatingRepository).Assembly);
            // addscoped makes the service is created once per http request
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDatingRepository, DatingRepository>();
            //specify authentication scheme that were using
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //if we re not in development mode then use global exception handler
                 app.UseExceptionHandler(builder => {
                         builder.Run(async context => {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                             var error = context.Features.Get<IExceptionHandlerFeature>();
                             if (error != null)
                             {
                                 context.Response.AddApplicationError(error.Error.Message);
                                 await context.Response.WriteAsync(error.Error.Message);
                             }
                         });
                 });

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //this is a strict security transport header
                //app.UseHsts();
            }

            //add CORS support as middleware
            //app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication(); 
            app.UseMvc(routes =>
            {
                // configures a root that is bypassed if a route is static
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    // we give it a new controller to look for
                    // api will know where index.html is when it goes to a route dat it doesn't recognize
                    defaults: new {controller = "Fallback", action = "Index"}
                );
            });
            // this looks for index html files
            app.UseDefaultFiles();
            // this code will serve static files
            app.UseStaticFiles();

        }
    }
}
