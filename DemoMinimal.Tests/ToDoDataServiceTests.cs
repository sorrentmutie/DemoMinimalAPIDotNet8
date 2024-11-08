using DemoAPI.Data;
using DemoMinimal.Core.DTO;
using DemoMinimalAPIDotNet8.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimal.Tests;

public class ToDoDataServiceTests
{
    [Fact]
    public async Task GetToDoReturnsNotFoundIfIDNotExists()
    {
        var mockDb = new MockDb();
        var context = mockDb.CreateDbContext();
        var service = new ToDoDataService(context);
        var id = 1;
        var result = await service.GetAsyncById(id);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetToDoReturnsToDOIfIDExists()
    {
        var mockDb = new MockDb();
        var context = mockDb.CreateDbContext();
        var service = new ToDoDataService(context);
        var id = 2;
        var idDb = await service.AddAsync(new ToDoDTO { Nome = "Test 1", Completato = false });
        var todos = await service.GetAsync();
        var result = await service.GetAsyncById(id);
        Assert.IsType<ToDoDTO>(result);
    }


    [Fact]
    public async Task GetTodosTest()
    {
        var mockDb = new MockDb();
        var context = mockDb.CreateDbContext();
        var service = new ToDoDataService(context);

        var newTodo = new ToDoDTO { Nome = "Test 1", Completato = false };
        await service.AddAsync(newTodo);

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
