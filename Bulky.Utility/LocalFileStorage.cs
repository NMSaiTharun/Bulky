using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BulkyBook.Utility
{
    /// <summary>
    /// Writes product images under wwwroot. Used for local development only — on App Service
    /// these files are replaced by every deployment.
    /// </summary>
    public class LocalFileStorage : IFileStorage
    {
        private const string RelativeFolder = "images/product";
        private readonly IWebHostEnvironment _environment;

        public LocalFileStorage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveAsync(IFormFile file, CancellationToken ct = default)
        {
            string productPath = Path.Combine(_environment.WebRootPath, "images", "product");
            Directory.CreateDirectory(productPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            await using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream, ct);
            }

            return $"/{RelativeFolder}/{fileName}";
        }

        public Task DeleteAsync(string? imageUrl, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) || Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                return Task.CompletedTask;
            }

            string relative = imageUrl.Replace('\\', '/').TrimStart('/');
            string fullPath = Path.Combine(_environment.WebRootPath, relative.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
    }
}
