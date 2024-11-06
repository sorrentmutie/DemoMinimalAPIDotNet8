using DemoAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimalAPIDotNet8.ExtensionsMethods;

public static class ToDoItemsExtensions
{

    public static void RegisterEndpoints(this WebApplication app)
    {
        
    
        var toDoGroup = app.MapGroup("/todoitems");

        toDoGroup.MapGet("/",
            async Task<Results<Ok<List<ToDo>>, NotFound>>
            (ToDoDb db) => await db.ToDos.ToListAsync()
                    is List<ToDo> todos
                        ? TypedResults.Ok(todos)
                        : TypedResults.NotFound()

            );
        //.Produces<List<ToDo>>(StatusCodes.Status200OK)
        //.Produces(StatusCodes.Status404NotFound)


        toDoGroup.MapGet("/completed", async (ToDoDb db) =>
              await db.ToDos.Where(t => t.IsComplete).ToListAsync())
        .WithOpenApi();

        toDoGroup.MapGet("/{id}", async (int id, ToDoDb db) =>
           await db.ToDos.FindAsync(id)
             is ToDo toDo ?
                Results.Ok(toDo) : Results.NotFound()
        );

        toDoGroup.MapPost("/", async (ToDo newToDo, ToDoDb db) => {
            db.ToDos.Add(newToDo);
            await db.SaveChangesAsync();
            //return Results.NoContent();
            return Results.Created($"/todoitems/{newToDo.Id}", newToDo);
        });

        toDoGroup.MapPut("/{id}", async
            (int id, ToDo inputToDo, ToDoDb db) => {

                // if(id != inputToDo.Id) return Results.BadRequest();

                var todo = await db.ToDos.FindAsync(id);
                if (todo is null) return Results.NotFound();
                todo.Name = inputToDo.Name;
                todo.IsComplete = inputToDo.IsComplete;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

        //toDoGroup.MapPatch("/{id}", async (int id, ToDo inputToDo, ToDoDb db) => {

        //    var todo = await db.ToDos.FindAsync(id);
        //    if (todo is null) return Results.NotFound();

        //    if(inputToDo.Name != null)
        //    {
        //        todo.Name = inputToDo.Name;
        //    }

        //    todo.IsComplete = inputToDo.IsComplete;
        //    await db.SaveChangesAsync();
        //    return Results.NoContent();

        //});

        toDoGroup.MapDelete("/{id}", async (int id, ToDoDb db) =>
        {
            var todo = await db.ToDos.FindAsync(id);
            if (todo is null) return Results.NotFound();
            db.ToDos.Remove(todo);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });


    }

    static async Task<Results<Ok<List<ToDo>>, NotFound>>
       GetAllTodos(ToDoDb db) => await db.ToDos.ToListAsync()
               is List<ToDo> todos
                   ? TypedResults.Ok(todos)
                   : TypedResults.NotFound();
}
