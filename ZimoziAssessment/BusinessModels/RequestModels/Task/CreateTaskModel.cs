using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zimozi.Assessment.Models;

namespace Zimozi.Assessment.BusinessModels.RequestModels.Response
{
    public class CreateTaskModel
    {

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime DeadlineDate {  get; set; }
    }
}
