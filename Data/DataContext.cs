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
                .WithMany(g => g.AddedFriends)
                .HasForeignKey(h => h.FriendOneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendShip>()
                .HasOne(f => f.FriendTwo)
                .WithMany(g => g.FriendsAdded)
                .HasForeignKey(h => h.FriendTwoId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>()
            //    .HasMany(f => f.AddedFriends)
            //    .WithOne(g => g.FriendOne)
            //    .HasForeignKey(w => w.FriendOneId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>()
            //    .HasMany(f => f.FriendsAdded)
            //    .WithOne(g => g.FriendTwo)
            //    .HasForeignKey(w => w.FriendTwoId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }

        //public DbSet<FriendRequest> FriendRequests { get; set; }

    }
}