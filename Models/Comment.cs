using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Guariba.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Autor do comentário
        public int UserId { get; set; }
        public User User { get; set; }

        // Post ao qual pertence
        public int PostId { get; set; }
        public Post Post { get; set; }

        // (Opcional) Comentário pai, para threads
        public int? ParentCommentId { get; set; }
        [ValidateNever]
        public Comment ParentComment { get; set; }
        [ValidateNever]
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
