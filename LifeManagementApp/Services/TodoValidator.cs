namespace LifeManagementApp.Services;

// Validointi erikseen ViewModelista -> voidaan testata xUnitilla
public static class TodoValidator
{
    public const int MinPriority = 1;
    public const int MaxPriority = 5;

    // Palauttaa virheviestin, tai null jos kaikki ok
    public static string? Validate(string? title, int priority)
    {
        // IsNullOrWhiteSpace hylkää myös pelkät välilyönnit "   "
        if (string.IsNullOrWhiteSpace(title))
            return "Title cannot be empty.";

        if (priority < MinPriority || priority > MaxPriority)
            return $"Priority must be between {MinPriority} and {MaxPriority}.";

        return null;
    }
}
