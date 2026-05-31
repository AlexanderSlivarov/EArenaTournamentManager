using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class UploadController : BaseController
    {
        private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/jpg",
            "image/png",
            "image/gif",
            "image/webp"
        };

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".webp"
        };

        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Image(IFormFile file, string category = "general")
        {
            if (!IsLoggedIn())
            {
                return Unauthorized(new { error = "Not logged in." });
            }

            if (file is null || file.Length == 0)
            {
                return BadRequest(new { error = "No file provided." });
            }

            if (!AllowedContentTypes.Contains(file.ContentType ?? string.Empty))
            {
                return BadRequest(new { error = "Only image files are allowed." });
            }

            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                return BadRequest(new { error = "Only image files are allowed." });
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new { error = "File size cannot exceed 5MB." });
            }

            category = string.IsNullOrWhiteSpace(category) ? "general" : category.Trim().ToLowerInvariant();
            if (!category.All(character => char.IsLetterOrDigit(character) || character == '-' || character == '_'))
            {
                return BadRequest(new { error = "Invalid upload category." });
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", category);
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension.ToLowerInvariant()}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Ok(new { url = Url.Content($"~/uploads/{category}/{fileName}") });
        }
    }
}

