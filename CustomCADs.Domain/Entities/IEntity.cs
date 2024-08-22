namespace CustomCADs.Domain.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set;  }
    }
}
