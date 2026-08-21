using Microsoft.AspNetCore.Http;

namespace BulkyBook.Utility
{
    /// <summary>
    /// Stores product images outside of the deployment content, so uploads survive a redeploy.
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>Saves the file and returns the URL to render it with.</summary>
        Task<string> SaveAsync(IFormFile file, CancellationToken ct = default);

        /// <summary>Deletes a previously saved file. Unknown or legacy paths are ignored.</summary>
        Task DeleteAsync(string? imageUrl, CancellationToken ct = default);
    }
}
