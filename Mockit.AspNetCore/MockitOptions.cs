namespace Mockit.AspNetCore
{
    public class MockitOptions
    {
        public TimeSpan? RefreshTime { get; set; } = null;

        public string UiPrefix { get; set; } = "/mockit";
    }
}
