namespace Suteki.Common.Services
{
    public interface IOrderableService<T>
        where T : class, IOrderable
    {
        IOrderServiceWithPosition<T> MoveItemAtPosition(int postion);
        int NextPosition { get; }
    }
}