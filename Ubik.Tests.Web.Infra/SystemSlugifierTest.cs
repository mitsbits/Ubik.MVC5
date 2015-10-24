using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Ubik.Web.Infra.Contracts;
using Ubik.Web.Infra.Services;

namespace Ubik.Tests.Web.Infra
{
    [TestClass]
    public class SystemSlugifierTest
    {
        private  ISlugifier _slugifier;
        [TestInitialize]
        public void SetUp()
        {
            var asciis = new List<IInternationalCharToAsciiProvider>(new[] {new GreekToAsciiProvider(),});
            var worders = new List<ISlugWordReplacer>(new[] {new SystemSlugWordRplacer(),});
            var ommiters = new List<ISlugCharOmmiter>(new[] {new SystemSlugCharReplacer(),});
            _slugifier = new SystemSlugService(worders, ommiters, asciis);
        }


        [TestMethod]
        public void TestMethod1()
        {
            _slugifier.ShouldNotBeNull();
            _slugifier.SlugifyText("Hello vanilla face!").ShouldBe("hello-vanilla-face");
        }


    }
}
