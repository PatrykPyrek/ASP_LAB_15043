using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("Subtasks")]
    public class SubtaskEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string Status { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; } 
        public TaskEntity Task { get; set; }
    }
}
