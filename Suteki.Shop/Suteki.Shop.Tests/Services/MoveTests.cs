using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using Suteki.Common;
using Suteki.Common.Repositories;
using Suteki.Common.Services;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class MoveTests
    {
        [Test]
        public void MoveUpOne_ShouldMoveAnItemUpOnePlace()
        {
            IQueryable<Thing> things = MakeSomeThings().AsQueryable();
            Assert.AreEqual("three", things.ElementAt(2).Name, "things are not in expected initial order");

            Move<Thing>.ItemAt(3).In(things).UpOne();
            IEnumerable<Thing> orderedThings = things.InOrder();

            Assert.AreEqual("three", orderedThings.ElementAt(1).Name, "Things have not been reordered");
        }

        [Test]
        public void MoveDownOne_ShouldMoveAnItemDownOnePlace()
        {
            IQueryable<Thing> things = MakeSomeThings().AsQueryable();
            Assert.AreEqual("two", things.ElementAt(1).Name, "things are not in expected initial order");

            Move<Thing>.ItemAt(2).In(things).DownOne();
            IEnumerable<Thing> orderedThings = things.InOrder();

            Assert.AreEqual("two", orderedThings.ElementAt(2).Name, "Things have not been reordered");
        }

        [Test]
        public void MovingTheTopItemUpOne_ShouldHaveNoEffect()
        {
            IQueryable<Thing> things = MakeSomeThings().AsQueryable();
            Assert.AreEqual("one", things.ElementAt(0).Name, "things are not in expected initial order");

            Move<Thing>.ItemAt(1).In(things).UpOne();
            IEnumerable<Thing> orderedThings = things.InOrder();

            Assert.AreEqual("one", orderedThings.ElementAt(0).Name, "Things have not been reordered");
        }

        [Test]
        public void MovingTheBottomItemDownOne_ShouldHaveNoEffect()
        {
            IQueryable<Thing> things = MakeSomeThings().AsQueryable();
            Assert.AreEqual("four", things.ElementAt(3).Name, "things are not in expected initial order");

            Move<Thing>.ItemAt(4).In(things).DownOne();
            IEnumerable<Thing> orderedThings = things.InOrder();

            Assert.AreEqual("four", orderedThings.ElementAt(3).Name, "Things have not been reordered");
        }

        public static IEnumerable<Thing> MakeSomeThings()
        {
            IEnumerable<Thing> things = new Thing[]
            {
                new Thing { Name = "one", Position = 1, FooId = 2 },
                new Thing { Name = "two", Position = 2, FooId = 1 },
                new Thing { Name = "three", Position = 3, FooId = 1 },
                new Thing { Name = "four", Position = 4, FooId = 2 }
            };

            return things;
        }
    }


    public class Thing : IOrderable
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public int FooId { get; set; }
    }
}
