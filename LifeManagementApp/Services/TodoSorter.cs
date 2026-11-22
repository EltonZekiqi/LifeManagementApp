using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

// Kaikki järjestyslogiikka yhessä paikassa (Home ja To-do-sivu käyttää samaa) -> ei duplikaattii
public static class TodoSorter
{
    // Top 3: vaan keskeneräset, ensin tärkein, sit aikasin deadline
    public static List<TodoItem> GetTop3(IEnumerable<TodoItem> todos)
    {
        return SortByPriority(todos.Where(t => !t.IsDone))
            .Take(3)
            .ToList();
    }

    // Prioriteetti isoin ensin, tasapelissä aikasin päivä ensin. Tehdyt aina listan loppuun
    public static List<TodoItem> SortByPriority(IEnumerable<TodoItem> todos)
    {
        return todos
            .OrderBy(t => t.IsDone)
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToList();
    }

    // Aikasin päivä ensin, tasapelissä tärkein ensin. Tehdyt loppuun
    public static List<TodoItem> SortByDueDate(IEnumerable<TodoItem> todos)
    {
        return todos
            .OrderBy(t => t.IsDone)
            .ThenBy(t => t.DueDate)
            .ThenByDescending(t => t.Priority)
            .ToList();
    }
}
