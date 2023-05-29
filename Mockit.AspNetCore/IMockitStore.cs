namespace Mockit.AspNetCore
{
    public interface IMockitStore
    {
        Task<ICollection<HttpMockEntity>> GetMocksAsync();

        Task SaveMockAsync(HttpMockEntity mock);

        Task<bool> IsRefreshRequired(int currentMockCount, DateTime? currentLastModified);
    }
}
