namespace LifeManagementApp.Models;

// Kategoria, esim. "School". Yhellä kategorialla voi olla monta to-doo
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // "Many"-puoli relaatiosta
    public List<TodoItem> Todos { get; set; } = new();

    // Picker näyttää tän tekstin listassa
    public override string ToString() => Name;
}
