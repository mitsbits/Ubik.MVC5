using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using Ubik.Web.Infra.Contracts;
using Ubik.Web.Infra.Services;

namespace Ubik.Tests.Web.Infra
{
    [TestFixture]
    public class SystemSlugifierTest
    {
        private ISlugifier _slugifier;

        [SetUp]
        public void SetUp()
        {
            var asciis = new List<IInternationalCharToAsciiProvider>(new[] { new GreekToAsciiProvider(), });
            var worders = new List<ISlugWordReplacer>(new[] { new SystemSlugWordRplacer(), });
            var ommiters = new List<ISlugCharOmmiter>(new[] { new SystemSlugCharReplacer(), });
            _slugifier = new SystemSlugService(worders, ommiters, asciis);
        }

        [TestCase("Hello vanilla face!", "hello-vanilla-face")]
        [TestCase("No shit!@#@!", "no-shit")]
        public void test_that_panctuation_is_removed(string source, string result)
        {
            _slugifier.ShouldNotBeNull();
            _slugifier.SlugifyText(source).ShouldBe(result);
        }

        [TestCase("Hello        vanilla                 face!", "hello-vanilla-face")]
        [TestCase("No        shit           !@#@!", "no-shit")]
        public void test_that_repeated_spaces_are_removed(string source, string result)
        {
            _slugifier.ShouldNotBeNull();
            _slugifier.SlugifyText(source).ShouldBe(result);
        }

        [TestCase("!@#$", "")]
        [TestCase("...", "")]
        [TestCase("~!)(*&^%$#@", "")]
        [TestCase("-", "")]
        [TestCase("_-=+/", "")]
        public void test_that_invalid_inputs_produce_empty_string(string source, string result)
        {
            _slugifier.ShouldNotBeNull();
            _slugifier.SlugifyText(source).ShouldBe(result);
        }
    }
}