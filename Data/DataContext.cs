using Microsoft.EntityFrameworkCore;
using soapApi.Models;

namespace soapApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FriendShip>()
                .HasKey(f => new { f.FriendOneId, f.FriendTwoId });

            modelBuilder.Entity<User>()
                .Ignore(e => e.Friends);

            modelBuilder.Entity<User>()
                .HasMany(f => f.MyRequests)
                .WithOne(r => r.Sender)
                .HasForeignKey(g => g.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(f => f.OthersRequests)
                .WithOne(r => r.Receiver)
                .HasForeignKey(g => g.ReceiverId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendShip>()
                .HasOne(f => f.FriendOne)
                .WithMany("FriendsOne")
                .HasForeignKey(h => h.FriendOneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendShip>()
                .HasOne(f => f.FriendTwo)
                .WithMany("FriendsTwo")
                .HasForeignKey(h => h.FriendTwoId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }

        //public DbSet<FriendRequest> FriendRequests { get; set; }

    }
}