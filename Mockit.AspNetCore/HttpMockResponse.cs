namespace Mockit.AspNetCore
{
    public class HttpMockResponse
    {
        public HttpMockResponse(int statusCode, Dictionary<string, string> headers, byte[]? content)
        {
            StatusCode = statusCode;
            Headers = headers;
            Content = content;
        }

        public int StatusCode { get; }

        public Dictionary<string, string> Headers { get; }

        public byte[]? Content { get; }
    }
}
