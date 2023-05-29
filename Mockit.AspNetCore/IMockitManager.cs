namespace Mockit.AspNetCore
{
    public interface IMockitManager
    {
        ICollection<HttpMock> GetMocks();

        Task SaveMockAsync(HttpMock mock);

        Task RefreshMocksAsync();

        Task RefreshMocksIfRequiredAsync();
    }
}
