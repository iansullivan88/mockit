using System.Net;

namespace Mockit.AspNetCore
{
    public class HttpMockResponse
    {
        public HttpMockResponse(HttpStatusCode statusCode, Dictionary<string, string> headers, byte[] content)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
            this.Content = content;
        }

        public HttpStatusCode StatusCode { get; }

        public Dictionary<string, string> Headers { get; }

        public byte[] Content { get; }
    }
}
