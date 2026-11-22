using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;

namespace LifeManagementApp.ViewModels;

// To-do-listan logiikka: lataus, järjestys, lisäys ja muokkaukseen siirtyminen
public partial class TodoListViewModel : ObservableObject
{
    private readonly ITodoRepository _todoRepository;

    private List<TodoItem> _allTodos = new();
    private bool _sortByPriority = true;

    public TodoListViewModel(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    // ObservableCollection kertoo CollectionViewlle kun rivejä lisätään/poistetaan
    public ObservableCollection<TodoItem> Todos { get; } = new();

    [ObservableProperty]
    private string sortLabel = "Sorted by priority (highest first)";

    [RelayCommand]
    public async Task LoadAsync()
    {
        _allTodos = await _todoRepository.GetAllAsync();
        ApplySort();
    }

    [RelayCommand]
    private void SortByPriority()
    {
        _sortByPriority = true;
        ApplySort();
    }

    [RelayCommand]
    private void SortByDueDate()
    {
        _sortByPriority = false;
        ApplySort();
    }

    // Uus to-do: avataan lomake ilman parametria
    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(AppRoutes.AddEditTodo);
    }

    // Muokkaus: viedään to-don Id parametrina lomakesivulle
    [RelayCommand]
    private async Task EditAsync(TodoItem item)
    {
        var parameters = new Dictionary<string, object>
        {
            [AddEditTodoViewModel.TodoIdKey] = item.Id
        };
        await Shell.Current.GoToAsync(AppRoutes.AddEditTodo, parameters);
    }

    private void ApplySort()
    {
        var sorted = _sortByPriority
            ? TodoSorter.SortByPriority(_allTodos)
            : TodoSorter.SortByDueDate(_allTodos);

        SortLabel = _sortByPriority
            ? "Sorted by priority (highest first)"
            : "Sorted by due date (earliest first)";

        // Tyhjennetään ja täytetään uudestaan järjestyksessä
        Todos.Clear();
        foreach (var todo in sorted)
            Todos.Add(todo);
    }
}
