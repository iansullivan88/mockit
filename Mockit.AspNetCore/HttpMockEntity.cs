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
        Dictionary<string, string> ResponseHeaders,
        string? ResponseContentBase64)
    {
    }
}
