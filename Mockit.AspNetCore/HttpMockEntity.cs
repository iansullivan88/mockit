namespace Mockit.AspNetCore
{
    /// <summary>
    /// An easily serializable representation of an http mock
    /// </summary>
    public record class HttpMockEntity(
        Guid Id,
        DateTime LastModified,
        bool Enabled,
        string Method,
        string Host,
        string Path,
        int ResponseStatusCode,
        string? ResponseContentBase64,
        List<HttpMockHeader> ResponseHeaders)
    {
    }
}
