namespace Mockit.AspNetCore
{
    public class HttpMockMatching
    {
        public HttpMockMatching(bool enabled, string method, string host, string path)
        {
            Enabled = enabled;
            Method = method;
            Host = host;
            Path = path;
        }

        public bool Enabled { get; }

        public string Method { get; }
        
        public string Host { get; }

        public string Path { get; }
    }
}
