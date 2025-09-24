using Microsoft.Extensions.Hosting;

namespace Guariba.Models
{
    public class Share
    {
        public int Id { get; set; }

        public DateTime SharedAt { get; set; }

        // opcional: comentário do usuário (tipo "quote tweet")
        public string? Comment { get; set; }

        // --- Relacionamentos ---

        // quem fez o repost
        public int UserId { get; set; }
        public User User { get; set; }

        // qual post está sendo repostado
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
