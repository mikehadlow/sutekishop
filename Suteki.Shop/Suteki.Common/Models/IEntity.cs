namespace Suteki.Common.Models
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface INamedEntity : IEntity
    {
        string Name { get; set; }
    }
}