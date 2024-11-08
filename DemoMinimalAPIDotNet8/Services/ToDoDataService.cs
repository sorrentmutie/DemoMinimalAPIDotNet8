using DemoAPI.Data;
using DemoMinimal.Core.DTO;
using DemoMinimal.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimalAPIDotNet8.Services;

public class ToDoDataService : IToDoData
{
    private readonly ToDoDb database;

    public ToDoDataService(ToDoDb database)
    {
        this.database = database;
    }

    public async Task<int> AddAsync(ToDoDTO todo)
    {
        var newTodo = new ToDo() { 
            Name = todo.Nome,
            IsComplete = todo.Completato
        };

        await database.ToDos.AddAsync(newTodo);
        await database.SaveChangesAsync();
        return newTodo.Id;
    }

    public async Task<List<ToDoDTO>?> GetAsync()
    {
        return await database.ToDos
            .Select(x => new ToDoDTO
            {
                Id = x.Id,
                Nome = x.Name,
                Completato = x.IsComplete
            })
            .ToListAsync();
    }
}
