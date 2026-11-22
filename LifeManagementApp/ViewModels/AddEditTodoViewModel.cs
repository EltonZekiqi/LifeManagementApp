using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;

namespace LifeManagementApp.ViewModels;

// Sama sivu hoitaa uuden lisäyksen JA muokkauksen. Jos tulee TodoId -> muokkaustila
public partial class AddEditTodoViewModel : ObservableObject, IQueryAttributable
{
    public const string TodoIdKey = "TodoId";

    private readonly ITodoRepository _todoRepository;

    private int? _editId;          // null = uus to-do
    private bool _initialized;

    public AddEditTodoViewModel(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private int priority = 3;

    [ObservableProperty]
    private DateTime dueDate = DateTime.Today.AddDays(1);

    [ObservableProperty]
    private string? notes;

    [ObservableProperty]
    private bool isDone;

    [ObservableProperty]
    private Category? selectedCategory;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string pageTitle = "New to-do";

    [ObservableProperty]
    private bool isEditMode;       // Delete-nappi näkyy vaan muokatessa

    // Pickerin vaihtoehdot 1, 2, 3, 4, 5
    public List<int> Priorities { get; } = new() { 1, 2, 3, 4, 5 };

    public ObservableCollection<Category> Categories { get; } = new();

    // Shell kutsuu tätä navigoinnissa ja antaa parametrit (esim. TodoId)
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(TodoIdKey, out var value) && value is int id)
        {
            _editId = id;
            IsEditMode = true;
            PageTitle = "Edit to-do";
        }
    }

    // Sivu kutsuu tätä OnAppearingissa
    public async Task InitializeAsync()
    {
        if (_initialized)
            return;           // Ladataan vaan kerran, ettei käyttäjän muutokset katoo
        _initialized = true;

        Categories.Clear();
        foreach (var category in await _todoRepository.GetCategoriesAsync())
            Categories.Add(category);

        if (_editId is null)
            return;           // Uus to-do -> lomake jää tyhjäks

        var item = await _todoRepository.GetByIdAsync(_editId.Value);
        if (item is null)
            return;

        // Täytetään lomake kannan tiedoilla
        Title = item.Title;
        Priority = item.Priority;
        DueDate = item.DueDate;
        Notes = item.Notes;
        IsDone = item.IsDone;
        SelectedCategory = Categories.FirstOrDefault(c => c.Id == item.CategoryId);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        // Tyhjä otsikko ei kelpaa
        var error = TodoValidator.Validate(Title, Priority);
        if (error is not null)
        {
            ErrorMessage = error;
            return;
        }

        var item = new TodoItem
        {
            Id = _editId ?? 0,               // 0 = kanta antaa uuden Id:n
            Title = Title.Trim(),
            Priority = Priority,
            DueDate = DueDate,
            Notes = Notes,
            IsDone = IsDone,
            CategoryId = SelectedCategory?.Id // Vaan vierasavain, ei koko Category-oliota
        };

        if (_editId is null)
            await _todoRepository.AddAsync(item);
        else
            await _todoRepository.UpdateAsync(item);

        await Shell.Current.GoToAsync("..");  // ".." = takas edelliselle sivulle
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (_editId is null)
            return;

        await _todoRepository.DeleteAsync(_editId.Value);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
