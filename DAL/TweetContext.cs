using TweetService.Models;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace TweetService.DAL
{
    public class TweetContext : DbContext
    {
        public TweetContext(DbContextOptions<TweetContext> options): base(options)
        {
            
        }
        public DbSet<Tweet> Tweets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(false);
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tweet>().ToTable("Tweet");
            modelBuilder.Entity<Tweet>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
