using FileManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagementApi.Data
{
    /// Contexto de la base de datos para la aplicación.
    /// Gestiona la conexión con SQLite y los mapeos de entidades.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// Tabla de metadatos de archivos.
        public DbSet<FileMetadata> Files { get; set; }

        /// Tabla de usuarios.
        public DbSet<User> Users { get; set; }

        /// Tabla de archivos compartidos.
        public DbSet<FileManagementApi.Models.FileShare> FileShares { get; set; }
    }
}
