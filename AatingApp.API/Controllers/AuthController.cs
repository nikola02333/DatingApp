using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AatingApp.API.Data;
using AatingApp.API.Dtos;
using AatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;

namespace AatingApp.API.Controllers {
    
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController (IAuthRepository repo,
         IConfiguration config,
         IMapper mapper) {
            this._config = config;
            this._repo = repo;
            this._mapper = mapper;
        }
        [HttpPost ("register")]
        // DTO = Data Transder Object,  mapiranje  objekta
        // od klijenta koji je u Json objektu  ka User-u 
        // Kroz Http stize fromBody, moze a i ne mora 
        public async Task<IActionResult> Register ([FromBody] UserForRegisterDto userFOrRegisterDto) {
            //validate request
            userFOrRegisterDto.Username = userFOrRegisterDto.Username.ToLower ();
            if (await _repo.UserExists (userFOrRegisterDto.Username))
                return BadRequest ("Username already exists");

            var userToCreate = _mapper.Map<User>(userFOrRegisterDto);
            var createdUser = await _repo.Register (userToCreate, userFOrRegisterDto.Password);
            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

            return CreatedAtRoute("GetUser", new { Controller= "users", id = createdUser.Id},userToReturn );
            //  CreatedAtRoute()
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (UserForLoginDto userForLoginDto) 
        {
           // throw new Exception("neka greska!!!");

            var userFromRepo = await _repo.Login (userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null) {
                return Unauthorized ();
            }
            //pravimo JWT token
            // User ID i Username 
            var claims = new [] {
                new Claim (ClaimTypes.NameIdentifier, userFromRepo.Id.ToString ()),
                new Claim (ClaimTypes.Name, userFromRepo.Username)

            };
            // ovoaj kljuc moramo definisati u AppSetings-u
            var key = new SymmetricSecurityKey (Encoding.UTF8
                            .GetBytes (_config.GetSection("appSettings:Token").Value));
            
            //Potpis  tokena
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires= DateTime.Now.AddDays(1),
                SigningCredentials = creds  
            };
            var tockenHandler = new JwtSecurityTokenHandler();
            var token = tockenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListDto>(userFromRepo);
            return Ok(new {
                // token, tako ce se zvati na Angularu, Kljuc:vrednost
                // token: eyJhb...
                token= tockenHandler.WriteToken(token),
                user
            });
            }
            
    }
}