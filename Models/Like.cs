using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Guariba.Models
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // Autor do like
        public int UserId { get; set; }
        public User User { get; set; }

        // Post curtido
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
