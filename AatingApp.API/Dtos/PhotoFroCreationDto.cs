using System;
using Microsoft.AspNetCore.Http;

namespace AatingApp.API.Dtos
{
    public class PhotoFroCreationDto
    {
       public string Url { get; set; }
       public IFormFile File { get; set; } 

       public string Description { get; set; }
       public DateTime DateAdded { get; set; }
       public string PublicId { get; set; }
       public PhotoFroCreationDto()
       {
           this.DateAdded = DateTime.Now;
       }
    }
}