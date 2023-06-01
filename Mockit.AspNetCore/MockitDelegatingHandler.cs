using System.Net;

namespace Mockit.AspNetCore
{
    public class MockitDelegatingHandler : DelegatingHandler
    {
        private readonly IMockMatcher matcher;

        public MockitDelegatingHandler(IMockMatcher matcher)
        {
            this.matcher = matcher;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var mock = this.matcher.GetMatchingMock(request);

            if (mock == null)
            {
                // don't mock this request
                return base.SendAsync(request, cancellationToken);
            }

            // build a mocked response
            var response = new HttpResponseMessage();
            response.StatusCode = (HttpStatusCode)mock.Response.StatusCode;
            
            foreach(var header in mock.Response.Headers)
            {
                response.Headers.Add(header.Name, header.Value);
            }

            if (mock.Response.Content != null)
            {
                response.Content = new ByteArrayContent(mock.Response.Content);
            }

            return Task.FromResult(response);
        }
    }
}
