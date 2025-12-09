using FileManagementApi.Models;
using FileManagementApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileManagementApi.Controllers
{
    /// API REST para la gestión de archivos y carpetas.
    /// Requiere cabecera X-User-Id para identificar al usuario.
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        private int GetUserId()
        {
            if (Request.Headers.TryGetValue("X-User-Id", out var userIdValue) && int.TryParse(userIdValue, out var userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found in headers.");
        }

        /// Sube un archivo al servidor.
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] int? parentId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            try
            {
                var userId = GetUserId();
                var metadata = await _fileService.UploadFileAsync(file, parentId, userId);
                return Ok(metadata);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// Crea una nueva carpeta.
        [HttpPost("folder")]
        public async Task<IActionResult> CreateFolder([FromForm] string name, [FromForm] int? parentId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Folder name is required.");
            }

            try
            {
                var userId = GetUserId();
                var metadata = await _fileService.CreateFolderAsync(name, parentId, userId);
                return Ok(metadata);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// Descarga un archivo individual.
        [HttpGet("{id}")]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var userId = GetUserId();
                var (stream, metadata) = await _fileService.DownloadFileAsync(id, userId);

                if (metadata == null)
                {
                    return NotFound("File not found.");
                }

                if (stream == null)
                {
                    return NotFound("File not found on disk.");
                }

                return File(stream, metadata.ContentType, metadata.FileName);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// Descarga una carpeta como archivo ZIP.
        [HttpGet("folder/{id}/download")]
        public async Task<IActionResult> DownloadFolder(int id)
        {
            try
            {
                var userId = GetUserId();
                var (stream, fileName) = await _fileService.DownloadFolderAsZipAsync(id, userId);
                return File(stream, "application/zip", fileName);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Lista archivos y carpetas, con opción de filtrado.
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int? parentId, [FromQuery] bool starredOnly = false)
        {
            try
            {
                var userId = GetUserId();
                var files = await _fileService.ListFilesAsync(parentId, userId, starredOnly);
                return Ok(files);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// Elimina un archivo o carpeta.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _fileService.DeleteFileAsync(id, userId);
                if (!result)
                {
                    return NotFound("File not found.");
                }
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// Alterna el estado de "destacado" de un archivo.
        [HttpPost("{id}/star")]
        public async Task<IActionResult> ToggleStar(int id)
        {
            try
            {
                var result = await _fileService.ToggleFileStarAsync(id);
                if (!result)
                {
                    return NotFound("File not found.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// Comparte un archivo con otro usuario.
        [HttpPost("{id}/share")]
        public async Task<IActionResult> ShareFile(int id, [FromForm] string username)
        {
            try
            {
                var userId = GetUserId();
                if(string.IsNullOrWhiteSpace(username)) return BadRequest("Username is required");

                var result = await _fileService.ShareFileAsync(id, username, userId);
                if (!result)
                {
                    return BadRequest("Could not share file. User not found or you don't own the file.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// Obtiene los archivos compartidos con el usuario actual.
        [HttpGet("shared")]
        public async Task<IActionResult> GetSharedFiles()
        {
            try
            {
                var userId = GetUserId();
                var files = await _fileService.ListSharedFilesAsync(userId);
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
