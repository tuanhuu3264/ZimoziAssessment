using System.ComponentModel.DataAnnotations;
using Zimozi.Assessment.Models;

namespace Zimozi.Assessment.BusinessModels.ResponseModels.User
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public Role Role { get; set; }
    }
}
