using Guariba.Models;
using Microsoft.EntityFrameworkCore;

namespace Guariba.Data
{
    public class GuaribaContext : DbContext
    {
        public GuaribaContext(DbContextOptions<GuaribaContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Share> Shares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações da tabela N:N UserFollow
            modelBuilder.Entity<UserFollow>()
                .HasKey(uf => new { uf.FollowerId, uf.FolloweeId });

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)       // quem segue
                .WithMany(u => u.Follows)        // coleção de quem segue
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Followee)       // quem é seguido
                .WithMany(u => u.Followers)      // coleção de seguidores
                .HasForeignKey(uf => uf.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
