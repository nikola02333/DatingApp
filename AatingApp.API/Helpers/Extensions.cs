using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        public static void AddPagination(this HttpResponse response , int currentPage, int itemsPerPage, int totalItems, int totalPages){

            var pagginationHeader = new PaginationHeader(  currentPage, itemsPerPage,totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("pagination", JsonConvert.SerializeObject(pagginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
            
            
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