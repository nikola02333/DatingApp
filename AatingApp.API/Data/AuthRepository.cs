using System.Threading.Tasks;
using AatingApp.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AatingApp.API.Data
{
    public class AuthRepository : IAuthRepository {
        private readonly DataContext _context;
        public AuthRepository (DataContext context) {
            this._context = context;
        }
        public  async Task<User> Login (string username, string password) {

            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Username== username);

            if(user== null)
               return null;

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))                
                return null; //ako vracamo null, to je kao da vracamo
                             // 401 unothorise
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using( var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
             var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
             // Posto je ComputeHash byte[] , moracemo da prodjemo kroz foor petlju 
             // i da poredimo savaki sa svakim
             for(int i = 0; i< ComputeHash.Length;i++)  
             {
                 if(ComputeHash[i] != passwordHash[i])
                   return false;
             }
            }
            return true;
        }

        public async Task<User> Register (User user, string password) {
        
            byte[] passwordHash,passwordSalt;
            CreatePasswordHash(password,out  passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt= passwordSalt;
            
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        public async Task<bool> UserExists (string username) {

            if(await _context.Users.AnyAsync(x=>x.Username == username))
            return true;

            return false;
        }
    }
}