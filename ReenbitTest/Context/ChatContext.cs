using Microsoft.EntityFrameworkCore;
using ReenbitTest.Entities;

namespace ReenbitTest.Context
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}

