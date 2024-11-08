using DemoAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimal.Tests;

public class MockDb: IDbContextFactory<ToDoDb>
{
    public ToDoDb CreateDbContext() {

        var options = new DbContextOptionsBuilder<ToDoDb>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        return new ToDoDb(options);
    }
}
