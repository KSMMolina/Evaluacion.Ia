namespace Api_test_ia.Application.Common
{
    public sealed record IncomingFile(
    string FileName,
    string ContentType,
    long Length,
    Stream Content
);
}
