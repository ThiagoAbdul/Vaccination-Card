namespace Domain.Interfaces;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; } // false é o default

}
