using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AatingApp.API.Data;
using AatingApp.API.Dtos;
using AatingApp.API.Helpers;
using AatingApp.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using  System.Collections.Generic;

namespace AatingApp.API.Controllers {
    [ServiceFilter (typeof (LogUserActivity))]
    [Authorize]
    [Route ("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessageController (IDatingRepository repository, IMapper mapper) {
            this._mapper = mapper;
            this._repo = repository;
        }
        [HttpGet("{id}", Name="GetMessage")]
        public async Task<IActionResult> GetMessage (int userId, int id) {

              if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messageFromRepo = await _repo.GetMessage(id);

            if(messageFromRepo !=null) {
                return NotFound();
            }
            return Ok(messageFromRepo);

        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreateionDtoo messageForCreateionDto) {
            
            var sender = await _repo.GetUser(userId);

             if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreateionDto.SenderId = userId;

            var recipient = await _repo.GetUser(messageForCreateionDto.RecipientId);
            if(recipient == null)
                return BadRequest("Could not find user");
            
            var message = _mapper.Map<Message>(messageForCreateionDto);

            _repo.Add(message);

         
            if(await  _repo.SaveAll()) {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetMessage", new {id = message.Id}, messageToReturn);
            }
            throw new Exception("Creating the message failed on save");


        }
        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser (int userId,
                [FromQuery] MessageParams messageParams) {
           
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

        messageParams.UserId = userId;
        var messageFromRepo = await _repo.GetMessageForUser(messageParams);

        var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);
        
        Response.AddPagination(messageFromRepo.CurrentPage,messageFromRepo.PageSize,
                               messageFromRepo.TotalCount, messageFromRepo.TotalPages);
        
        return Ok(messages);
        }
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetmessageThread(int userId, int RecipientId) {

              if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromRepo = await _repo.GetMessageThread(userId, RecipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messageThread);

        }
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage (int id, int UserId) {
            
             if (UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messageFormRepo = await _repo.GetMessage(id);

            if(messageFormRepo.SenderId ==UserId)
                messageFormRepo.SenderDeleted=true;
            
            if(messageFormRepo.RecipientId == UserId)
                messageFormRepo.RecipientDeleted= true;
            
            if( messageFormRepo.SenderDeleted && messageFormRepo.RecipientDeleted)
                _repo.Delete(messageFormRepo);
            
            if(await _repo.SaveAll())
                return NoContent();
            
            throw new Exception("Error deleting the message");
        } 
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
              if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
             var message = await _repo.GetMessage(id);
             
             if( message.RecipientId != userId)
                return Unauthorized();

             message.IsRead =true;
             message.DateRead = DateTime.Now;

             await _repo.SaveAll();
             
             return NoContent();
        }
       
    }
}