namespace Mockit.AspNetCore
{
    public class HttpMockResponse
    {
        public HttpMockResponse(int statusCode, List<HttpMockHeader> headers, byte[]? content)
        {
            StatusCode = statusCode;
            Headers = headers;
            Content = content;
        }

        public int StatusCode { get; }

        public List<HttpMockHeader> Headers { get; }

        public byte[]? Content { get; }
    }
}
