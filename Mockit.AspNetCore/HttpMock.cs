namespace Mockit.AspNetCore
{
    public sealed class HttpMock
    {
        public HttpMock(
            Guid id,
            HttpMockMatching matching,
            HttpMockResponse response,
            DateTime lastModified) 
        {
            Id = id;
            Matching = matching;
            Response = response;
            LastModified = lastModified;
        }

        public Guid Id { get; }

        public HttpMockMatching Matching { get; }

        public HttpMockResponse Response { get; }

        public DateTime LastModified { get; }

        public static HttpMockEntity ToEntity(HttpMock mock)
        {
            return new HttpMockEntity
            (
                Id: mock.Id,
                LastModified: mock.LastModified,
                Enabled: mock.Matching.Enabled,
                Method: mock.Matching.Method,
                Host: mock.Matching.Host,
                Path: mock.Matching.Path,
                ResponseStatusCode: mock.Response.StatusCode,
                ResponseContentBase64: mock.Response.Content == null
                    ? null
                    : Convert.ToBase64String(mock.Response.Content),
                ResponseHeaders: mock.Response.Headers
            );
        }

        public static HttpMock FromEntity(HttpMockEntity entity)
        {
            return new HttpMock(
                id: entity.Id,
                matching: new HttpMockMatching(
                    enabled: entity.Enabled,
                    method: entity.Method,
                    host: entity.Host,
                    path: entity.Path),
                response: new HttpMockResponse(
                    statusCode: entity.ResponseStatusCode,
                    headers: entity.ResponseHeaders,
                    content: entity.ResponseContentBase64 == null
                        ? null
                        : Convert.FromBase64String(entity.ResponseContentBase64)),
                lastModified: entity.LastModified);
        }
    }
}
