using System.Security.Cryptography;
using System.Text;
using FileManagementApi.Data;
using FileManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagementApi.Services
{

    /// Interfaz para el servicio de autenticación.
    public interface IAuthService
    {

        /// Registra un nuevo usuario.
        Task<User?> RegisterAsync(string username, string password);
        
        /// Valida credenciales de usuario.
        Task<User?> LoginAsync(string username, string password);
    }

    /// Implementación del servicio de autenticación.
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> RegisterAsync(string username, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                return null;
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return null;
            }

            if (user.PasswordHash != HashPassword(password))
            {
                return null;
            }

            return user;
        }

        /// Genera un hash SHA256 de la contraseña.
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
