namespace Mockit.AspNetCore
{
    public class HttpMockMatching
    {
        public HttpMockMatching(bool enabled, HttpMethod method, string host, string path)
        {
            Enabled = enabled;
            Method = method;
            Host = host;
            Path = path;
        }

        public bool Enabled { get; }

        public HttpMethod Method { get; }
        
        public string Host { get; }

        public string Path { get; }
    }
}
