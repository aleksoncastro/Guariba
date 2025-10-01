using Guariba.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guariba.Data
{
    public class SocialMediaContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public SocialMediaContext (DbContextOptions<SocialMediaContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Comment> Comment { get; set; } = default!;
        public DbSet<Like> Like { get; set; } = default!;
        public DbSet<PersonalInformation> PersonalInformation { get; set; } = default!;
        public DbSet<Post> Post { get; set; } = default!;
        public DbSet<PrivateMessage> PrivateMessage { get; set; } = default!;
        public DbSet<Share> Share { get; set; } = default!;
        public DbSet<UserFollow> UserFollow { get; set; } = default!;

        public DbSet<UserInterest> UserInterest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // ----- Configuração da Relação Muitos-para-Muitos (UserFollow) -----
            modelBuilder.Entity<UserFollow>(entity =>
            {
                // Define a chave primária composta
                entity.HasKey(uf => new { uf.FollowerId, uf.FolloweeId });

                // Relação: Um "Follower" (seguidor) pode ter muitos "Followees" (pessoas que ele segue)
                entity.HasOne(uf => uf.Follower)
                      .WithMany(u => u.Follows) // A coleção em User.cs que representa quem o usuário segue
                      .HasForeignKey(uf => uf.FollowerId)
                      .OnDelete(DeleteBehavior.Restrict); // Evita problemas de exclusão em cascata

                // Relação: Um "Followee" (seguido) pode ter muitos "Followers" (seguidores)
                entity.HasOne(uf => uf.Followee)
                      .WithMany(u => u.Followers) // A coleção em User.cs que representa os seguidores
                      .HasForeignKey(uf => uf.FolloweeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ----- Configuração da Relação Dupla 1-para-Muitos (PrivateMessage) -----
            modelBuilder.Entity<PrivateMessage>(entity =>
            {
                // Relação para o remetente (Sender)
                entity.HasOne(pm => pm.Sender)
                      .WithMany(u => u.SentMessages) // A coleção de mensagens enviadas no User.cs
                      .HasForeignKey(pm => pm.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relação para o destinatário (Recipient)
                entity.HasOne(pm => pm.Recipient)
                      .WithMany(u => u.ReceivedMessages) // A coleção de mensagens recebidas no User.cs
                      .HasForeignKey(pm => pm.RecipientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração para a entidade UserInterest
            modelBuilder.Entity<UserInterest>(entity =>
            {
                // Define a chave primária composta por UserId e o enum Interest
                entity.HasKey(ui => new { ui.UserId, ui.Interest });

                // Configura a relação com User
                entity.HasOne(ui => ui.User)
                      .WithMany(u => u.UserInterests)
                      .HasForeignKey(ui => ui.UserId);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                // Quebra o caminho em cascata direto entre User e Comment
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Mude de Cascade para Restrict
            });

            modelBuilder.Entity<Like>(entity =>
            {
                // Breaks the direct cascade path from User to Like
                entity.HasOne(l => l.User)
                      .WithMany(u => u.LikesGiven)
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Change the default from Cascade
            });


            // Para a entidade Share
            modelBuilder.Entity<Share>(entity =>
            {
                entity.HasOne(s => s.User)
                      .WithMany(u => u.Shares)
                      .HasForeignKey(s => s.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Quebra o caminho User -> Share
            });

            // Definição de tabela para cada entidade
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Like>().ToTable("Like");
            modelBuilder.Entity<PersonalInformation>().ToTable("PersonalInformation");
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Share>().ToTable("Share");
            
        }
    }
}
