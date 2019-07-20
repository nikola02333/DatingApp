using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers {
    
    [Authorize] // sve unutar ovog kontrlera mora biti AUTORIZOVSAN zahtev

    [Route ("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        private readonly DataContext _dataContext;
        public ValuesController (DataContext dataContext) {
            this._dataContext = dataContext;

        }
        // GET api/values
        [HttpGet]
         public async Task<IActionResult> GetValues() {

            var values=await _dataContext.Values.ToListAsync();
            return  Ok(values);
        }

        [AllowAnonymous] // ne treba da imamo JWT token,
                         // ne treba da budemo loginovani :D
                         // Za ovo nam treba Middlwer,
                         //  njega ukljucujemo u 
                         // Startap klasi, ConfigurationServices  
        // GET api/values/5
        [HttpGet ("{id}")]
         public async Task<IActionResult> GetValue (int id) {
            
        var value= await _dataContext.Values.FirstOrDefaultAsync(x =>x.Id==id);
        return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post ([FromBody] string value) { }

        // PUT api/values/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value) { }

        // DELETE api/values/5
        [HttpDelete ("{id}")]
        public void Delete (int id) { }
    }
}