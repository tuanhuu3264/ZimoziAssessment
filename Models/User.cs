using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zimozi.Assessment.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        public string? Email { get; set; }
        [Required]
        [StringLength(50)]
        public string? Password { get; set; }
        public Role Role { get; set; }
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
        public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
    }
    public enum Role
    {
        Admin,
        User
    }
}
