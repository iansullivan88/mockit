using System.Collections.Concurrent;

namespace Mockit.AspNetCore
{
    public class InMemoryMockitStore : IMockitStore
    {
        private readonly ConcurrentDictionary<Guid, HttpMockEntity> _mocks = new ConcurrentDictionary<Guid, HttpMockEntity>();
        
        public InMemoryMockitStore()
        { 
        }

        public Task<ICollection<HttpMockEntity>> GetMocksAsync()
        {
            return Task.FromResult(_mocks.Values);
        }

        public Task SaveMockAsync(HttpMockEntity mock)
        {
            _mocks[mock.Id] = mock;

            return Task.CompletedTask;
        }
        public Task<bool> IsRefreshRequired(int currentMockCount, DateTime? currentLastModified)
        {
            var numberOfMocksChanged = currentMockCount != _mocks.Count;
            var firstMockAdded = currentLastModified == null && _mocks.Any();
            var changesAvailable = numberOfMocksChanged
                || firstMockAdded
                || _mocks.Values.Any(m => m.LastModified > currentLastModified);

            return Task.FromResult(changesAvailable);
        }
    }
}
