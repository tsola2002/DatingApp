using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    //route url resolves to api/users
    // we use authorize to make sure that anybody using the controller is authorized
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // we initialize properties from parameter
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        // private readonly IMapper _mapper;

        // we create our constructor here
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
           // _mapper = mapper;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            // we add a variable that will get our users
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            // we return ok and send back the users
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            // we use our mapper to map the source object(user) to destination object(UserForDetailed)
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            // if the token username does not match the path username
            // return a 401 unauthorized request
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // get the user id
            var userFromRepo = await _repo.GetUser(id);

            // get information from userUpdateDto and map it to userFromRepo
            // it takes the information and writes it to the user from repo
            _mapper.Map(userForUpdateDto, userFromRepo);

            // then we save our changes
            if (await _repo.SaveAll())
                return NoContent();
            
            // if something has gone wrong with our save then we throw an exception
            throw new Exception($"Updating user {id} failed on save");
        }
    }
}