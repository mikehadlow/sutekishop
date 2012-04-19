using System.Linq;
using NUnit.Framework;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class OrderableServiceTests
    {
        IRepository<Thing> thingRepository;
        IOrderableService<Thing> orderService;

        [SetUp]
        public void SetUp()
        {
            thingRepository = MockRepository.GenerateStub<IRepository<Thing>>();
            orderService = new OrderableService<Thing>(thingRepository);
        }

        [Test]
        public void MoveUpShouldMoveElementUp()
        {
            var things = MoveTests.MakeSomeThings().AsQueryable();

            thingRepository.Expect(tr => tr.GetAll()).Return(things);

            // move two up to top
            orderService.MoveItemAtPosition(2).UpOne();

            Assert.AreEqual("two", things.Single(t => t.Position == 1).Name);
        }

        [Test]
        public void MoveDownShouldMoveElementDown()
        {
            var things = MoveTests.MakeSomeThings().AsQueryable();

            thingRepository.Expect(tr => tr.GetAll()).Return(things);

            // move three down to bottom
            orderService.MoveItemAtPosition(3).DownOne();

            Assert.AreEqual("three", things.Single(t => t.Position == 4).Name);
        }

        [Test]
        public void MoveUpShouldNotMoveElementUpWhenConstrainedByFooId()
        {
            var things = MoveTests.MakeSomeThings().AsQueryable();

            thingRepository.Expect(tr => tr.GetAll()).Return(things);

            orderService.MoveItemAtPosition(2).ConstrainedBy(thing => thing.FooId == 1).UpOne();

            Assert.AreEqual("two", things.Single(t => t.Position == 2).Name);
        }

        [Test]
        public void MoveDownShouldNotMoveWhenConstrainedByFooId()
        {
            var things = MoveTests.MakeSomeThings().AsQueryable();

            thingRepository.Expect(tr => tr.GetAll()).Return(things);

            orderService.MoveItemAtPosition(3).ConstrainedBy(thing => thing.FooId == 1).DownOne();

            Assert.AreEqual("three", things.Single(t => t.Position == 3).Name);
        }
    }
}
