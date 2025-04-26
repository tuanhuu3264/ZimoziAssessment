using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zimozi.Assessment.Models
{
    public class Task
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime DeadlineDate { get; set; }

        public DateTime? CompletedDate { get; set; }
        [Required]
        public Status Status { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? ChargedUser { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
    }
    public enum Status
    {
        Success,
        Failure,
        Pending
    }
}
