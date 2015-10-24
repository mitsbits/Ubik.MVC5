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

        [TestCase("_-va=+li=.*/d", "va-li-d")]
        [TestCase("This title is long! Don't you think?", "this-title-is-long-dont-you-think")]
        public void test_that_slugs_are_valid(string source, string result)
        {
            _slugifier.ShouldNotBeNull();
            _slugifier.SlugifyText(source).ShouldBe(result);
        }

        [TestCase("Γεια σας, είμαι τίτλος", "geia-sas-eimai-titlos")]
        [TestCase("Ω Θεέ μου! Τι ωραία που είναι αυτά που γράφεις", "o-thee-mou-ti-oraia-pou-einai-auta-pou-grafeis")]
        public void test_that_greek_input_produces_valid_slug(string source, string result)
        {
            _slugifier.ShouldNotBeNull();
            _slugifier.SlugifyText(source).ShouldBe(result);
        }
    }
}