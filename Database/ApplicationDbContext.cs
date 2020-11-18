using Microsoft.EntityFrameworkCore;
using RascalChatApp.Models;

namespace RascalChatApp.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
