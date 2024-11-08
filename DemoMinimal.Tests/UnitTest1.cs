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