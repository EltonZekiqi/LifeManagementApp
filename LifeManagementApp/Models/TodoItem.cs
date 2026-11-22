namespace LifeManagementApp.Models;

// Yks to-do rivi. EF Core tekee tästä taulun "Todos" (Code First)
public class TodoItem
{
    public int Id { get; set; }                     // Pääavain, EF Core arvaa nimestä "Id"
    public string Title { get; set; } = string.Empty;
    public int Priority { get; set; } = 3;          // 1 = matala, 5 = tärkein
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);
    public bool IsDone { get; set; }
    public string? Notes { get; set; }

    // Relaatio: monta to-doo voi kuulua yhteen kategoriaan (Category 1–N TodoItem)
    public int? CategoryId { get; set; }            // Vierasavain, null = ei kategoriaa
    public Category? Category { get; set; }         // Navigaatio-property
}
