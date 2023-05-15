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
    }
}
