namespace Mockit.AspNetCore
{
    public interface IMockMatcher
    {
        public HttpMock? GetMatchingMock(HttpRequestMessage request);

        public void Rebuild(ICollection<HttpMock> mocks);
    }
}
