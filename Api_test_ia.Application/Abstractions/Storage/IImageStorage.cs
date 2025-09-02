namespace Api_test_ia.Application.Abstractions.Storage
{
    public interface IImageStorage
    {
        /// Guarda un archivo dentro de un contenedor (p.ej. "products/123") y devuelve una ruta relativa web (/uploads/...).
        Task<string> SaveAsync(string fileName, Stream data, string contentType, string container, CancellationToken ct);

        /// Elimina el archivo físico si existe. La ruta debe ser la devuelta por SaveAsync (relativa web, empieza con "/").
        Task DeleteAsync(string relativeWebPath, CancellationToken ct);
    }
}
