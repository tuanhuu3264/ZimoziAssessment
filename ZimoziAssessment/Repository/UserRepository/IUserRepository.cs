using Zimozi.Assessment.Models;

namespace Zimozi.Assessment.Repository.UserRepository
{
    public interface IUserRepository
    {
        public Task<User> Login( string username, string password);
    }
}
