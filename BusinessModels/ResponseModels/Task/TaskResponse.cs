using System.ComponentModel.DataAnnotations;
using Zimozi.Assessment.BusinessModels.ResponseModels.User;
using Zimozi.Assessment.Models;

namespace Zimozi.Assessment.BusinessModels.ResponseModels.Task
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
       
        public DateTime CreatedDate { get; set; }
       
        public DateTime DeadlineDate { get; set; }

        public DateTime? CompletedDate { get; set; }
       
        public Status Status { get; set; }

        public int UserId { get; set; }
        public UserResponse? userResponse { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
