using System.Collections.Generic;
using System.Threading.Tasks;
using AatingApp.API.Helpers;
using AatingApp.API.Models;

namespace AatingApp.API.Data
{
    public interface IDatingRepository
    {
         void Add<T> (T entity) where T: class;
         // umesto Pravljenja 2 metode za Add User-a i Photo
         // pravimo 1 metodu koja ce oba posla raditi
         void Delete<T> (T entity) where T: class;

        Task<bool> SaveAll();

        Task<PagedList<User>> GetUsers(UserParams userParams);

        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);

        Task<Photo> GetMainPhotoForUser(int usrerId);
        
    }
}