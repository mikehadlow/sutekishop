using System;
using System.Collections.Generic;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;

namespace Suteki.Common.Services
{
    public class Move<T> : IMoveItems<T>, IMoveDirection where T : IOrderable
    {
        private IEnumerable<T> items;
        readonly private int position;

        public static IMoveItems<T> ItemAt(int position)
        {
            var move = new Move<T>(position);
            return move;
        }

        public Move(int position)
        {
            this.position = position;
        }

        IMoveDirection IMoveItems<T>.In(IEnumerable<T> items)
        {
            var move = new Move<T>(position) { items = items };
            return move;
        }

        void IMoveDirection.UpOne()
        {
            SwapPositionWith(items.GetItemBefore(position));
        }

        void IMoveDirection.DownOne()
        {
            SwapPositionWith(items.GetItemAfter(position));
        }

        private void SwapPositionWith(IOrderable swapItem)
        {
            if (swapItem == null) return;

            IOrderable item = items.AtPosition(position);
            if (item == null)
                throw new ApplicationException("There is no item at position {0}".With(position));

            // swap postions
            int tempPosition = swapItem.Position;
            swapItem.Position = item.Position;
            item.Position = tempPosition;
        }
    }

    public interface IMoveItems<T>
    {
        IMoveDirection In(IEnumerable<T> items);
    }

    public interface IMoveDirection
    {
        void UpOne();
        void DownOne();
    }
}