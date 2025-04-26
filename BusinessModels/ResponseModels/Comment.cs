using System.ComponentModel.DataAnnotations;

namespace Zimozi.Assessment.BusinessModels.ResponseModels
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }
}
