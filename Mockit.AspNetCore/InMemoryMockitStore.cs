using System.Collections.Concurrent;

namespace Mockit.AspNetCore
{
    public class InMemoryMockitStore : IMockitStore
    {
        private readonly ConcurrentDictionary<Guid, HttpMock> _mocks = new ConcurrentDictionary<Guid, HttpMock>();
        
        public InMemoryMockitStore()
        { 
        }

        public Task<ICollection<HttpMock>> GetMocksAsync()
        {
            return Task.FromResult(_mocks.Values);
        }

        public Task SaveMockAsync(HttpMock mock)
        {
            _mocks[mock.Id] = mock;

            return Task.CompletedTask;
        }
    }
}
