using Api_test_ia.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Api_test_ia.Infrastructure.Storage
{
    public sealed class FileSystemImageStorage(IWebHostEnvironment env, IOptions<FileStorageOptions> opt) : IImageStorage
    {
        private readonly string _webRoot = env.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot");
        private readonly string _root = (opt.Value.Root ?? "uploads").Trim('/');

        public async Task<string> SaveAsync(string fileName, Stream data, string contentType, string container, CancellationToken ct)
        {
            // carpeta relativa: uploads/products/123
            var relDir = Path.Combine(_root, container.Replace("\\", "/").Trim('/'));
            var absDir = Path.Combine(_webRoot, relDir);
            Directory.CreateDirectory(absDir);

            var absPath = Path.Combine(absDir, fileName);
            data.Position = 0;
            await using (var fs = new FileStream(absPath, FileMode.Create, FileAccess.Write, FileShare.None))
                await data.CopyToAsync(fs, ct);

            // ruta web: /uploads/products/123/filename.jpg
            var relWeb = "/" + Path.Combine(relDir, fileName).Replace("\\", "/");
            return relWeb;
        }

        public Task DeleteAsync(string relativeWebPath, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(relativeWebPath)) return Task.CompletedTask;
            var rel = relativeWebPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var full = Path.Combine(_webRoot, rel);
            if (File.Exists(full)) File.Delete(full);
            return Task.CompletedTask;
        }
    }
}
