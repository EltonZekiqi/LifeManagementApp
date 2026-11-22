using LifeManagementApp.Data;
using LifeManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeManagementApp.Services;

// Kaikki tietokantajutut on täällä. Jokanen metodi tekee oman lyhytikäsen DbContextin factoryllä
public class TodoRepository : ITodoRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    // DI antaa factoryn, ei tehdä itse "new"
    public TodoRepository(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<TodoItem>> GetAllAsync()
    {
        // await using = context suljetaan automaattisesti lopuksi
        await using var db = await _dbFactory.CreateDbContextAsync();

        // Include hakee kategorian mukaan (JOIN). AsNoTracking = vaan luetaan, nopeempi
        return await db.Todos
            .AsNoTracking()
            .Include(t => t.Category)
            .ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Todos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(TodoItem item)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Todos.Add(item);            // Merkataan "Added"
        await db.SaveChangesAsync();   // Vasta tässä tehään INSERT kantaan, ja item.Id saa arvon
    }

    public async Task UpdateAsync(TodoItem item)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        // Haetaan kannasta se oikea rivi ja kopioidaan uudet arvot siihen
        var existing = await db.Todos.FirstOrDefaultAsync(t => t.Id == item.Id);
        if (existing is null)
            return;

        existing.Title = item.Title;
        existing.Priority = item.Priority;
        existing.DueDate = item.DueDate;
        existing.IsDone = item.IsDone;
        existing.Notes = item.Notes;
        existing.CategoryId = item.CategoryId;

        await db.SaveChangesAsync();   // EF huomaa muutokset ja tekee UPDATEn
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var existing = await db.Todos.FindAsync(id);
        if (existing is null)
            return;

        db.Todos.Remove(existing);
        await db.SaveChangesAsync();
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
    }
}
