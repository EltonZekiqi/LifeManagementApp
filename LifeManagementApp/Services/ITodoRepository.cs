using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

// Sopimus datan käsittelylle. ViewModel tuntee vaan tän, ei EF Corea eikä SQLitee
public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task DeleteAsync(int id);
    Task<List<Category>> GetCategoriesAsync();
}
