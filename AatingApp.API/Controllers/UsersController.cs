using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AatingApp.API.Data;
using AatingApp.API.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AatingApp.API.Controllers
{
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
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repoo.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
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
    }
}