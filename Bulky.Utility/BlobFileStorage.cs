using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace BulkyBook.Utility
{
    /// <summary>
    /// Stores product images in Azure Blob Storage. Blob URLs are absolute, so the views
    /// render them unchanged and the files are untouched by deployments.
    /// </summary>
    public class BlobFileStorage : IFileStorage
    {
        private readonly BlobContainerClient _container;

        public BlobFileStorage(string connectionString, string containerName)
        {
            _container = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<string> SaveAsync(IFormFile file, CancellationToken ct = default)
        {
            await _container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: ct);

            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            BlobClient blob = _container.GetBlobClient(blobName);

            await using Stream stream = file.OpenReadStream();
            await blob.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
            }, ct);

            return blob.Uri.ToString();
        }

        public async Task DeleteAsync(string? imageUrl, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            // Legacy wwwroot paths (\images\product\...) are not ours to delete.
            if (!imageUrl.StartsWith(_container.Uri.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string blobName = Path.GetFileName(new Uri(imageUrl).AbsolutePath);
            await _container.GetBlobClient(blobName).DeleteIfExistsAsync(cancellationToken: ct);
        }
    }
}
