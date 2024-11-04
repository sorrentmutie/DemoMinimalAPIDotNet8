using DemoMinimalAPIDotNet8.Models;
using DemoMinimalAPIDotNet8.Services;
using Microsoft.EntityFrameworkCore;
using MyDbContext.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMyService, MyService>();

builder.Services.AddKeyedSingleton<IMyCache, BigCache>("big");
builder.Services.AddKeyedSingleton<IMyCache, SmallCache>("small");

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterTodoItemsEndpoints();

app.Run();


