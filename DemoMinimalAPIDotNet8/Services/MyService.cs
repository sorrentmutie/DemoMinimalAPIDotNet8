namespace DemoMinimalAPIDotNet8.Services;

public class MyService : IMyService
{
    private readonly ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }

    public string DoSomething(string message)
    {
        _logger.LogWarning("yyyy " + message);
        return $"xxxxxCiao, {message}";
    }
}
