namespace Mockit.AspNetCore
{
    public interface IMockitStore
    {
        Task<ICollection<HttpMock>> GetMocksAsync();

        Task SaveMockAsync(HttpMock mock);
    }
}
