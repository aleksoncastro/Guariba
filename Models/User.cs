using Guariba.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser<int>
{
    [Required]
    public DateTime RegistrationDat { get; set; }

    public string? ProfilePhoto { get; set; } // Corrigido para anulável

    public PersonalInformation? PersonalInformation { get; set; }

    public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();

    // --- Seguidores / Seguindo (N:N via UserFollow) ---
    public ICollection<UserFollow> Follows { get; set; } = new List<UserFollow>();
    public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();

    // --- Navegações inversas de Conteúdo ---
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Share> Shares { get; set; } = new List<Share>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> LikesGiven { get; set; } = new List<Like>();

    // --- Navegações inversas de Mensagens Privadas ---
    public ICollection<PrivateMessage> SentMessages { get; set; } = new List<PrivateMessage>();
    public ICollection<PrivateMessage> ReceivedMessages { get; set; } = new List<PrivateMessage>();
}