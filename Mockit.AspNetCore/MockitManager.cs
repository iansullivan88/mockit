namespace Mockit.AspNetCore
{
    public class MockitManager : IMockitManager
    {
        private readonly IMockitStore _store;
        private readonly IMockMatcher _matcher;

        private ICollection<HttpMock> _mocks = new List<HttpMock>();
        
        public MockitManager(
            IMockitStore store,
            IMockMatcher matcher)
        {
            _store = store;
            _matcher = matcher;
        }

        public ICollection<HttpMock> GetMocks()
        {
            return _mocks;
        }

        public async Task SaveMockAsync(HttpMock mock)
        {
            var entity = HttpMock.ToEntity(mock);
            await _store.SaveMockAsync(entity);
            await RefreshMocksAsync();
        }

        public async Task RefreshMocksIfRequiredAsync()
        {
            var mocks = _mocks;
            var lastModified = mocks.Any() ? (DateTime?)mocks.Max(m => m.LastModified) : null;
            
            var isRequired = await _store.IsRefreshRequired(_mocks.Count, lastModified);
            
            if (isRequired)
            {
                await RefreshMocksAsync();
            }
        }

        public async Task RefreshMocksAsync()
        {
            var entities = await _store.GetMocksAsync();
            lock (_mocks)
            {
                var mocks = entities
                    .Select(HttpMock.FromEntity)
                    .ToList();

                _matcher.Rebuild(mocks);
                _mocks = mocks;
            }
        }
    }
}
