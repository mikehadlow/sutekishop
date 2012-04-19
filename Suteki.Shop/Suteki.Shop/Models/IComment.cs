using Suteki.Common.Models;

namespace Suteki.Shop
{
    public interface IComment : IEntity
    {
        bool Approved { get; set; }
        string Text { get; set; }
        string Reviewer { get; set; }
        string Answer { get; set; }
        bool HasAnswer { get; }
    }
}