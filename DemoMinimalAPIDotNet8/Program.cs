using DemoAPI.Data;
using DemoMinimal.Core.Interfaces;
using DemoMinimalAPIDotNet8;
using DemoMinimalAPIDotNet8.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ToDoDb>(
    options => options.UseInMemoryDatabase("ToDoList"));
builder.Services.AddKeyedSingleton<IMyCache, BigCache>("big");
builder.Services.AddKeyedSingleton<IMyCache, SmallCache>("small");
builder.Services.AddScoped<IToDoData, ToDoDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(e =>
    e.Run(async context => await Results.Problem().ExecuteAsync(context)));


//app.UseExceptionHandler(e =>
//    e.Run(async context =>
//    {

//        await context.Response.WriteAsync("Mio Fallback");
//    }));
app.UseHttpsRedirection();

//app.MapGet("/big", ([FromKeyedServices("big")] IMyCache bigCache  ) => bigCache.GetCacheValue("date");
//app.MapGet("/small", ([FromKeyedServices("small")] IMyCache bigCache) => bigCache.GetCacheValue("date");

//app.MapGet("/exception", () => { 
//    throw new InvalidOperationException("This is an exception");
//});

//app.MapGet("/exception/{id}", (int id) => {
//    if(id <= 0) return Results.BadRequest("Id must be greater than 0");
//    if(id == 1) throw new ArgumentException("This is an exception");
//    return Results.Ok($"Id is {id}");
//})
//    .WithOpenApi( o => new OpenApiOperation
//    {
//        Summary = "Get an item by id",
//        Description = "Bla bla bla",
//        Deprecated = true,
//        Parameters = new List<OpenApiParameter>
//        {
//            new OpenApiParameter
//            {
//                Name = "id",
//                In = ParameterLocation.Path,
//                Required = true,
//                Schema = new OpenApiSchema
//                {
//                    Type = "integer",
//                    Format = "int32"
//                }
//            }
//        },
//    });


app.RegisterTodoItemsEndpoints();

app.Run();

