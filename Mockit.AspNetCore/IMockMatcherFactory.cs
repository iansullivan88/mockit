namespace Mockit.AspNetCore
{
    public interface IMockMatcherFactory
    {
        IMockMatcher Create(IEnumerable<HttpMock> mocks);
    }
}
