using Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("projects")]
    public class ProjectEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
