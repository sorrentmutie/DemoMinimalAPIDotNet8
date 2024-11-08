using DemoMinimal.Core.Interfaces;

namespace DemoMinimalAPIDotNet8.Services;

public class BigCache : IMyCache
{
    public string GetCacheValue(string key)
    {
        return $"returning {key} from Big Cache";
    }
}

public class SmallCache : IMyCache
{
    public string GetCacheValue(string key)
    {
        return $"returning {key} from Small Cache";
    }
}

