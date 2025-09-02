namespace Api_test_ia.Infrastructure.Storage
{
    public sealed class FileStorageOptions
    {
        /// Carpeta raíz bajo wwwroot (por defecto "uploads")
        public string Root { get; set; } = "uploads";
    }
}
