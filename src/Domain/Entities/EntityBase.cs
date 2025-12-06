namespace Domain.Entities;

public class EntityBase<ID> where ID : IEquatable<ID>
{
    public ID Id { get; set; }
}
