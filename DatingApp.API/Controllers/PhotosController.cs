using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    // we give it a route that will allow us to get to this controller
    //we need to set it to be an APiController
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController: ControllerBase
    {  
            private readonly IDatingRepository _repo;
            private readonly IMapper _mapper;
            private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
            private Cloudinary _cloudinary;

            public PhotosController(IDatingRepository repo, IMapper mapper,
                IOptions<CloudinarySettings> cloudinaryConfig)
            {
                _cloudinaryConfig = cloudinaryConfig;
                _mapper = mapper;
                _repo = repo;

                Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
                );

                _cloudinary = new Cloudinary(acc);
            }

        // this will take in the id of the photo & a route name(get photo)
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {   
            //we assign the getPhoto method from our repository
            var photoFromRepo = await _repo.GetPhoto(id);
            // we return a photforretunrdto which is mapped to photfromrepo
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            // we can now return ok and pass in our photo
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,
            [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            // we first compare the userId from the token against te root parameter of userid
            // if they dont match we return an unauthorized request
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // if it matches then fetch the user from the repo
            var userFromRepo = await _repo.GetUser(userId);
            // we create a variable to store the actual file
            var file = photoForCreationDto.File;
            // we create a variable to store the result we get from cloudinary
            var uploadResult = new ImageUploadResult();

            // we check to see that there something inside of the file
            if (file.Length > 0)
            {
                // we now load the file into the memory stream
                using (var stream = file.OpenReadStream())
                {
                    // we now give cloudinary our upload parameters
                    var uploadParams = new ImageUploadParams()
                    {
                        // we now specify the file details
                        File = new FileDescription(file.Name, stream),
                        // we need to transform the image incase a larger image is uploaded
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    // we now upload the file using cloudinary upload method
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            // we now need to pull the response from uploadResult
            // we populate the other fields inside our photos for creation dto
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            // we map photoforcreation dto to the photo itself based on the properties we now have
            var photo = _mapper.Map<Photo>(photoForCreationDto);

            // we check if the user has a main photo
            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            // add the photo
            userFromRepo.Photos.Add(photo);

            // check if it has saved all changes to the database
            if (await _repo.SaveAll())
            {
                // we create the mapped version of the photo to be returned
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                //return createdAtRoute response which will return a 201 resonponse location header
                // we also need a route to get an individual photo
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            //if it doesnt save we returna badRequest
            return BadRequest("Could not add the photo");
        }

        // this api will take in the id of the photo/setMain
        // we are using the id to set the isMain property of the photo to true
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            // we'll check if the user is authorized
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // we'll check that the id parameter matches the one from the photo
            // we first get the user from the repo
            var user = await _repo.GetUser(userId);

            // we make sure the photo exists in the users photo collection
            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            // get the photos from the repo
            var photoFromRepo = await _repo.GetPhoto(id);

            // well check to see that the photo were trying to affect is the main photo
            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            // we need to grab our current main photo from the repo
            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            // we set it to false
            currentMainPhoto.IsMain = false;


            // we change the user selected photo based on the id supplied to true
            photoFromRepo.IsMain = true;

            // we save all the changes
            if (await _repo.SaveAll())
                return NoContent();

            // if it fails we return bad request
            return BadRequest("Could not set photo to main");
        }      
        
    }
}