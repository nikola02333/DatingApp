using System.Collections.Generic;
using System.Threading.Tasks;
using AatingApp.API.Data;
using AatingApp.API.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AatingApp.API.Controllers {
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IDatingRepository _repoo;
        private readonly IMapper _mapper;
        public UsersController (IDatingRepository repo, IMapper mapper) {
            this._mapper = mapper;
            this._repoo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers () {
            var users = await _repoo.GetUsers ();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok (usersToReturn);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetUser (int id) {

            var user = await _repoo.GetUser (id);

            var userTOReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok (userTOReturn);
        }
    }
}