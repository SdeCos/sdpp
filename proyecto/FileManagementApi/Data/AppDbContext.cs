using FileManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagementApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FileMetadata> Files { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
