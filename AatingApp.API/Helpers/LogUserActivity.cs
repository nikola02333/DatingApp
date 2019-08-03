using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using AatingApp.API.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace AatingApp.API.Helpers
{
     public class LogUserActivity : IAsyncActionFilter
    {
         public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
        var resultContex = await next();
        var userId = int.Parse(resultContex.HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = resultContex.HttpContext.RequestServices.GetService<IDatingRepository>();
            // izvadili smo Koji je  iD je koriscen, i posle samo 
            // ubdejtujemo LastActive polje
            var user = await repo.GetUser(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}