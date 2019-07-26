using System;
using Microsoft.AspNetCore.Http;

namespace AatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error",message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");

        }
        public static int CalculateAge(this DateTime theDataTime){
         
         var age = DateTime.Today.Year - theDataTime.Year;
         if( theDataTime.AddYears(age) > DateTime.Today)
         {
             age--;
         }
         return age;
        }
    }
}