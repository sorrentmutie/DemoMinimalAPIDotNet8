using DemoAPI.Data;
using DemoMinimal.Core.DTO;
using DemoMinimal.Core.Interfaces;
using DemoMinimalAPIDotNet8;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace DemoMinimal.Tests
{
    public class ToDoEndpointsTests
    {
        [Fact]
        public async Task GetToDoById()
        {
            var id = 5;
            var mock = new Mock<IToDoData>();
            mock.Setup(m => m.GetAsyncById(id)).ReturnsAsync(
                new ToDoDTO
                { Id = id, Completato = true, Nome = "Test" }
            );

            var result = await TodoItemsEndpoints.GetTodoById(id, mock.Object);
            Assert.IsType<Results<Ok<ToDoDTO>, NotFound>>(result);
            var todo = (Ok<ToDoDTO>)result.Result;
            Assert.Equal(todo.Value?.Id, id);
        }

        [Fact]
        public async Task GetTodosTests()
        {
            var mock = new Mock<IToDoData>();
            mock.Setup(m => m.GetAsync()).ReturnsAsync(
                new List<ToDoDTO>
                {
                    new ToDoDTO {Id = 1, Nome = "Test1", Completato = false},
                    new ToDoDTO {Id = 2, Nome = "Test2", Completato = true}
                });


            var result = await TodoItemsEndpoints.GetTodos2(mock.Object);
            Assert.IsType<Ok<List<ToDoDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(result.Value.Count, 2);
        }
    }
}