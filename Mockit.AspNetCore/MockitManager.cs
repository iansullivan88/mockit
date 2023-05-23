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
            await _store.SaveMockAsync(mock);
            await RefreshMocksAsync();
        }

        public async Task RefreshMocksAsync()
        {
            var latestMocks = await _store.GetMocksAsync();
            lock (_mocks)
            {
                var latestMocksModified = latestMocks.Any() ? latestMocks.Max(m => m.LastModified) : DateTime.MinValue;
                var mocksModified = _mocks.Any() ? _mocks.Max(m => m.LastModified) : DateTime.MinValue;

                if (mocksModified != latestMocksModified)
                {
                    _matcher.Rebuild(latestMocks);
                    _mocks = latestMocks;
                }
            }
        }
    }
}
