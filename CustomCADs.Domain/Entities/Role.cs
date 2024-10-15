namespace CustomCADs.Domain.Entities;

public class Role
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<User> Users { get; set; } = [];
}
