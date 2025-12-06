using FileManagementApi.Models;
using FileManagementApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileManagementApi.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int? parentId)
        {
            try
            {
                var userId = GetUserId();
                var files = await _fileService.ListFilesAsync(parentId, userId);
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
    }
}
