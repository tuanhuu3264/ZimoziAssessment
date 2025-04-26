using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Zimozi.Assessment.Models
{
    public class TaskComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TaskId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("TaskId")]
        public virtual Task? Task { get; set; } 

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
