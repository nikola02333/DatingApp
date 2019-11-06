using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AatingApp.API.Data;
using AatingApp.API.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AatingApp.API.Helpers;
namespace AatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repoo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repoo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var curentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var  userFromRepo = await _repoo.GetUser(curentUserId);

            userParams.UserId = curentUserId;
            if( string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }
            var users = await _repoo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
          
             Response.AddPagination(users.CurrentPage,
                    users.PageSize, users.TotalCount,users.TotalPages);
           
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {

            var user = await _repoo.GetUser(id);

            var userTOReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userTOReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {

            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repoo.GetUser(id);
            _mapper.Map(userForUpdateDto, userFromRepo);
            // sa leva na desno, ubacuje nove podatke
            if (await _repoo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed to save");
        }
        [HttpPost("{id}/like/{recepientId}")]
        public async Task<ActionResult> LikeUser(int id, int recepientId) {

            if( id !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var like = await _repoo.GetLike(id, recepientId);
            if (like != null)
                return BadRequest("You Already like this user");

            if (await _repoo.GetUser(recepientId) == null)
               return NotFound();

            like = new Models.Like
            {
                LikerId = id,
                LikeeId = recepientId
            };
            _repoo.Add<Models.Like>(like);
            if (await _repoo.SaveAll())
                return Ok();

        return BadRequest("Failed to Like User");
        }
    }
}