using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
