using DemoAPI.Data;
using DemoMinimalAPIDotNet8.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimal.Tests;

public class ToDoDataServiceTests
{
    [Fact]
    public async Task GetTodosTest()
    {
        var mockDb = new MockDb();
        var context = mockDb.CreateDbContext();
        var service = new ToDoDataService(context);

        var newTodo = new ToDo { Name = "Test 1", IsComplete = false };
        context.ToDos.Add(newTodo);
        await context.SaveChangesAsync();
        context.Entry(newTodo).State = EntityState.Detached;

        var todos = await service.GetAsync();
        if(todos is not null)
        {
            Assert.Single(todos);
        } else
        {
            Assert.Fail();
        }

        context.ToDos.Remove(new ToDo { Id = 1 });
        await context.SaveChangesAsync();


        todos = await service.GetAsync();
        if (todos is not null)
        {
            Assert.Empty(todos);
        }
        else
        {
            Assert.Fail();
        }
    }
}
