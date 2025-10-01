namespace Guariba.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string TextContent { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // --- Relacionamentos ---
        public int UserId { get; set; }
        public User User { get; set; }

        // Counts
        public int? CommentsCount { get; set; }
        public int? LikesCount { get; set; }
        public int? RetweetsCount { get; set; }
        public int? SharesCount { get; set; }



    }
}