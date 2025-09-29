using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guariba.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDat { get; set; }

        public String ProfilePhoto { get; set; }

        public PersonalInformation PersonalInformation { get; set; }

        public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();

        // --- Seguidores / Seguindo (N:N via UserFollow) ---
        // TER QUE FAZER MANUALMENTE POIS O SCAFFOLDING NÃO FAZ AUTOMATICAMENTE
        //[NotMapped]
        public ICollection<UserFollow> Follows { get; set; } = new List<UserFollow>();
        // quem o usuário segue
        // [NotMapped]
        public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();   // quem segue o usuário


        // --- Navegações inversas de Conteúdo ---
        // Depois (com a correção)
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Share> Shares { get; set; } = new List<Share>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> LikesGiven { get; set; } = new List<Like>();
   

        // --- Navegações inversas de Mensagens Privadas ---
        //[NotMapped]
        public ICollection<PrivateMessage> SentMessages { get; set; } = new List<PrivateMessage>();
        // [NotMapped]
        public ICollection<PrivateMessage> ReceivedMessages { get; set; } = new List<PrivateMessage>();
    }
}
