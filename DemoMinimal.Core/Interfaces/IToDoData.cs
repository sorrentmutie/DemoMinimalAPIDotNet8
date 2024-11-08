using DemoMinimal.Core.DTO;

namespace DemoMinimal.Core.Interfaces;

public interface IToDoData
{
    Task<List<ToDoDTO>?> GetAsync();
    Task<ToDoDTO?> GetAsyncById(int id);
    Task<int> AddAsync(ToDoDTO todo);
}

public interface IGenericData<T> where T : class
{
    Task<List<T>?> GetAsync();
    Task<T?> GetAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
