using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("Tasks")]
    public class TaskEntity
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

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public ProjectEntity Project { get; set; }
    }
}
