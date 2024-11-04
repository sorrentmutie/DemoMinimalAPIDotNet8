namespace DemoMinimalAPIDotNet8.Services;

public interface IMyCache
{
    string Get(string key);
}
public class BigCache : IMyCache
{
    public string Get(string key) => $"Resolving {key} from big cache.";
}

public class SmallCache : IMyCache
{
    public string Get(string key) => $"Resolving {key} from small cache.";
}