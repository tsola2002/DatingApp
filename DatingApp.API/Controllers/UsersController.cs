using System.Collections.Generic;
using System.Threading.Tasks;
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
        // private readonly IMapper _mapper;

        // we create our constructor here
        public UsersController(IDatingRepository repo)
        {
           // _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            // we add a variable that will get our users
            var users = await _repo.GetUsers();

            //var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            // we return ok and send back the users
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            //var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(user);
        }
    }
}