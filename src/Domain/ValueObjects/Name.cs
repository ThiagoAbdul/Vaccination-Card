namespace Domain.ValueObjects;

public class Name
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

}
