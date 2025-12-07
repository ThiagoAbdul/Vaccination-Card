namespace Domain.ValueObjects;

public class Name
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(FirstName)) return false;
        if (string.IsNullOrWhiteSpace(LastName)) return false;

        if (FirstName.Length < 2 || LastName.Length < 2) return false;

        bool validFirst = FirstName.All(c => char.IsLetter(c) || c == ' ');
        bool validLast = LastName.All(c => char.IsLetter(c) || c == ' ');

        return validFirst && validLast;
    }

}
