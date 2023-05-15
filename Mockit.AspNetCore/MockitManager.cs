namespace Mockit.AspNetCore
{
    public class MockitManager
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
            await _store.SaveMockAsync(mock);
            await RefreshMocksAsync();
        }

        public async Task RefreshMocksAsync()
        {
            var latestMocks = await _store.GetMocksAsync();
            lock (_mocks)
            {
                if (latestMocks.Max(m => m.LastModified) > _mocks.Max(m => m.LastModified))
                {
                    _matcher.Rebuild(latestMocks);
                    _mocks = latestMocks;
                }
            }
        }
    }
}
