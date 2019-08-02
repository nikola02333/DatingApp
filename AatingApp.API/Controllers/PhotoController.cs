using AatingApp.API.Data;
using AatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using System.Threading.Tasks;
using AatingApp.API.Dtos;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using AatingApp.API.Models;
using System.Linq;

[Authorize]
[Route("api/users/{userId}/photos")]
[ApiController]
public class PhotoController : ControllerBase
{
    private readonly IDatingRepository _repository;
    private readonly IMapper _mapper;
    private readonly IOptions<CloudinarySettings> _cloudinarydotNet;
    private Cloudinary _cloudinary;

    public PhotoController(IDatingRepository repository,
                                IMapper mapper,
                                IOptions<CloudinarySettings> cloudinarydotNet)
    {

        this._cloudinarydotNet = cloudinarydotNet;
        this._mapper = mapper;
        this._repository = repository;

        Account acc = new Account(
                _cloudinarydotNet.Value.CloudName,
                _cloudinarydotNet.Value.ApiKey,
                _cloudinarydotNet.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }
    // Sad je Acc podesen 
    [HttpGet("{id}", Name = "GetPhoto")]
    public async Task<IActionResult> GetPhoto(int id)
    {
        var photoFromRepo = await _repository.GetPhoto(id);
        // ovde sad mi netrebaju informacije u korisniku, samo Slika
        var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

        return Ok(photo);
    }

    [HttpPost]
    public async Task<IActionResult> AddPhotoForUser(int UserId,[FromForm] PhotoFroCreationDto photoforCreateionDto)
    {
        if (UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
        var userFromRepo = await _repository.GetUser(UserId);

        var file = photoforCreateionDto.File;

        var uploadResult = new ImageUploadResult();
        if (file.Length > 0)
        {
            // cita sliku iz ram memorije
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation()
                    .Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = _cloudinary.Upload(uploadParams);
            }
        }
        photoforCreateionDto.Url = uploadResult.Uri.ToString();
        photoforCreateionDto.PublicId = uploadResult.PublicId;

        var photo = _mapper.Map<Photo>(photoforCreateionDto);

        if (!userFromRepo.Photos.Any(u => u.IsMain))
            photo.IsMain = true;

        userFromRepo.Photos.Add(photo);


        if (await _repository.SaveAll())
        {
            var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
            return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
        }
        return BadRequest("Could not add the photo");
    }
    [HttpPost ("{id}/setMain")]
    public async Task<IActionResult> SetMainPhoto(int userId, int id) {

        if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

        var user = await _repository.GetUser(userId);
        if(!user.Photos.Any(p => p.Id == id))
            return Unauthorized();
        
        var photoFromRepo = await _repository.GetPhoto(id);
        if(photoFromRepo.IsMain)
            return BadRequest("This id alredy the main photo");
        
        var currentMainPhoto = await _repository.GetMainPhotoForUser(userId);
        currentMainPhoto.IsMain = false;

        photoFromRepo.IsMain = true;
        if( await _repository.SaveAll())
            return NoContent();
        
        return BadRequest("could not set photo to main");
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(int userId, int id) {
    
    if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

    var user = await _repository.GetUser(userId);
    if(!user.Photos.Any(p => p.Id == id))
        return Unauthorized();
        
    var photoFromRepo = await _repository.GetPhoto(id);
    if(photoFromRepo.IsMain)
        return BadRequest("Ypu cannot delete your main photo");
    
    if(photoFromRepo.PublicId!= null)
    {
        var deleteParams = new DeletionParams(photoFromRepo.PublicId);
        var result = _cloudinary.Destroy(deleteParams);
        if(result.Result =="ok"){
            _repository.Delete(photoFromRepo);
        }
    }
    
    if(photoFromRepo.PublicId == null)
    {
        _repository.Delete(photoFromRepo);
    }
    if(await _repository.SaveAll())
    {
        return Ok();
    }
    return BadRequest("Failed to delete the photo");
            
    }
}


