// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.HtmlHelpers;
using Suteki.Common.Models;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.Tests.TestHelpers;

namespace Suteki.Common.Tests.HtmlHelpers
{
    [TestFixture]
    public class ComboForTests
    {
        private ComboFor<ComboForLookup, ComboForModel> comboFor;
        private IRepository<ComboForLookup> repository;

        [SetUp]
        public void SetUp()
        {
            repository = MockRepository.GenerateStub<IRepository<ComboForLookup>>();
            comboFor = new ComboFor<ComboForLookup, ComboForModel>(repository)
            {
                HtmlHelper = MvcTestHelpers.CreateMockHtmlHelper<ComboForModel>()
            };
        }

        [Test]
        public void BindTo_should_return_the_correct_select_list()
        {
            var ids = new[] {2, 3};
            var loopups = new System.Collections.Generic.List<ComboForLookup>()
            {
                new ComboForLookup {Id = 1, Name = "one"},
                new ComboForLookup {Id = 2, Name = "two"},
                new ComboForLookup {Id = 3, Name = "three"},
                new ComboForLookup {Id = 4, Name = "four"},
            };
            repository.Stub(x => x.GetAll()).Return(loopups.AsQueryable());

            var result = comboFor.Multiple().BoundTo("PropertyName", ids);
            //Console.Out.WriteLine("result = {0}", result);
            result.ShouldEqual(expectedSelectList);
        }

        private const string expectedSelectList = 
@"<select id=""PropertyName"" multiple=""multiple"" name=""PropertyName""><option value=""1"">one</option>
<option selected=""selected"" value=""2"">two</option>
<option selected=""selected"" value=""3"">three</option>
<option value=""4"">four</option>
</select>";
    }

    public class ComboForModel{}
    public class ComboForLookup : INamedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

// ReSharper restore InconsistentNaming
