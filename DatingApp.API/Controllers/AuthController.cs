using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{   

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        //we inject our repository & configuration settings into the controller
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //validate request 
            //if(!ModelState.IsValid)
                //return BadRequest(ModelState);

            //we convert username to lower case
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            //if the username already exists then we return a bad request
            if(await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            //if it doesnt exist then we create a new user
            var userToCreate = new User 
            {
                Username = userForRegisterDto.Username
            };    

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Username);

            return StatusCode(201);
          
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
           // try {
                
                //wen you manually throw an exception the rest of the code stops running
                //throw new Exception("Computer says no!");

                // we first retrieve the user from the repository
                // this gives us the full user object
                var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

                //if the user does not exist display a 401(Unauthorized request)
                if (userFromRepo == null) 
                return Unauthorized();

                //we now need to build a tokken that we'll return to the user
                //we create a claim variable which will store the claim name & its type
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username)
                };

                //we build a key to sign our token(to make sure that the token is valid)
                //we store the key as byte array and pull it from settings because we need to use it in
                //different places
                var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

                //with the key we create signing credentials
                //this will use the security key & algorithm
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                //we create the token descriptor which will add the claim name & expiry date
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                //We create a token handler
                var tokenHandler = new JwtSecurityTokenHandler();

                //with the handler, we create a token & pass in the token descriptor
                var token = tokenHandler.CreateToken(tokenDescriptor);

                //we map to userforlistDTO
                var user = _mapper.Map<UserForListDto>(userFromRepo);

                
                //we use the token handler to write our token as a response to our client
                return Ok(new {
                    token = tokenHandler.WriteToken(token),
                    //we send back the user as another property inside the anonymous object were already returning
                    user

                    

                });
        
            //}
           // catch {
           //     return StatusCode(500, "Computer really says no");
           // }
            
        }
    }

}    
