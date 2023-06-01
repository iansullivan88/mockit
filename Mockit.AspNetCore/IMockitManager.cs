namespace Mockit.AspNetCore
{
    public interface IMockitManager
    {
        List<HttpMock> GetMocks();

        Task SaveMockAsync(HttpMock mock);

        Task RefreshMocksAsync();

        Task RefreshMocksIfRequiredAsync();
    }
}
