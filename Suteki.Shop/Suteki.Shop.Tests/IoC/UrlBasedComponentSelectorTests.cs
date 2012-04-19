// ReSharper disable InconsistentNaming
using System;
using Castle.Core;
using Castle.MicroKernel;
using NUnit.Framework;
using Suteki.Shop.IoC;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.IoC
{
    [TestFixture]
    public class UrlBasedComponentSelectorTests
    {
        private UrlBasedComponentSelector urlBasedComponentSelector;

        [SetUp]
        public void SetUp()
        {
            urlBasedComponentSelector = new UrlBasedComonentSelectorTestProxy(typeof(ISomeSelectableType));
        }

        [Test]
        public void Should_have_opinion_about_a_selectable_type()
        {
            urlBasedComponentSelector.HasOpinionAbout("", typeof(ISomeSelectableType)).ShouldBeTrue();
        }

        [Test]
        public void Should_not_have_an_opinion_about_a_non_selectable_type()
        {
            urlBasedComponentSelector.HasOpinionAbout("", typeof (ISomeOtherType)).ShouldNotBeNull();
        }

        [Test]
        public void Should_return_the_correct_handler_for_matching_host()
        {
            var handler1 = BuildHandler("default");
            var handler2 = BuildHandler("ISomeSelectableType:the.host.name");
            var handlers = new[] {handler1, handler2};

            var selectedHandler = urlBasedComponentSelector.SelectHandler("", typeof (ISomeSelectableType), handlers);

            selectedHandler.ShouldBeTheSameAs(handler2);
        }

        [Test]
        public void Should_return_the_correct_handler_if_no_matching_host()
        {
            var handler1 = BuildHandler("default");
            var handler2 = BuildHandler("ISomeSelectableType:other.host.name");
            var handlers = new[] { handler1, handler2 };

            var selectedHandler = urlBasedComponentSelector.SelectHandler("", typeof(ISomeSelectableType), handlers);

            selectedHandler.ShouldBeTheSameAs(handler1);
        }

        private static IHandler BuildHandler(string key)
        {
            var handler = MockRepository.GenerateStub<IHandler>();
            var componentModel = new ComponentModel(key, typeof (ISomeSelectableType), typeof (ISomeSelectableType));
            handler.Stub(h => h.ComponentModel).Return(componentModel).Repeat.Any();
            return handler;
        }
    }

    public class UrlBasedComonentSelectorTestProxy : UrlBasedComponentSelector
    {
        public UrlBasedComonentSelectorTestProxy(params Type[] selectableTypes) : base(selectableTypes)
        {
        }

        protected override string GetHostname()
        {
            return "the.host.name";
        }
    }

    public interface ISomeSelectableType {}
    public interface ISomeOtherType {}
}
// ReSharper restore InconsistentNaming
