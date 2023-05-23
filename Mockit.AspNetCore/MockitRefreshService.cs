using Microsoft.Extensions.Hosting;

namespace Mockit.AspNetCore
{
    public class MockitRefreshService : IHostedService
    {
        private readonly MockitOptions _options;
        private readonly IMockitManager _manager;
        private readonly Timer _timer;

        public MockitRefreshService(MockitOptions options, IMockitManager manager) 
        {
            _options = options;
            _manager = manager;
            _timer = new Timer(TimerEvent);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.RefreshTime != null)
            {
                _timer.Change(0, (int)_options.RefreshTime.Value.TotalMilliseconds);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            return Task.CompletedTask;
        }

        private async void TimerEvent(object? state)
        {
            await _manager.RefreshMocksAsync();
        }
    }
}
