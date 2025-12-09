using FileManagementApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileManagementApi.Controllers
{
    /// Controlador para la gestión de autenticación de usuarios.
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// Registra un nuevo usuario en el sistema.
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] string username, [FromForm] string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = await _authService.RegisterAsync(username, password);
            if (user == null)
            {
                return BadRequest("User already exists.");
            }

            return Ok(new { user.Id, user.Username });
        }

        /// Inicia sesión y devuelve la información del usuario.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = await _authService.LoginAsync(username, password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { user.Id, user.Username });
        }
    }
}
