using Microsoft.EntityFrameworkCore;
using Zimozi.Assessment.Helpers;
using Zimozi.Assessment.Models;

namespace Zimozi.Assessment.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {   
        public Context context { get; set; }
        public UserRepository(Context context) { 
         this.context = context;
        }
        public async  Task<User> Login(string username, string password)
        {
            string hashPassword = Hash.HashPassword(password);
            var users = context?.Users?.ToList();
            return await context.Users.Where((user) => user.Email.ToLower() == username.ToLower() && user.Password == hashPassword).FirstOrDefaultAsync();
        }
    }
}
