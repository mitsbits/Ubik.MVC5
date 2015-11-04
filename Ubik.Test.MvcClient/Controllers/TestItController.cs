using System.Web.Mvc;
using Ubik.Web.Cms;
using Ubik.Web.Components;

namespace Ubik.Test.MvcClient.Controllers
{
    public class TestItController : Controller
    {
        // GET: TestLayOut
        public ActionResult Index()
        {
            //var content = new CmsContent() { Title = "Test Lay out" };
            //ViewBag.ComponentContent = content;

            var device = CmsPageDevice();
            ViewBag.Device = device;
            return View();
        }

        private static CmsDevice CmsPageDevice()
        {
            var device = new CmsDevice()
            {
                FriendlyName = "Empty",
                Path = string.Empty,
                Flavor = DeviceRenderFlavor.Empty
            };
            //create an action
            var action = new CmsPartialAction("Inner", "Inner", "TestIt", string.Empty);
            action.Parameters.Add("id", "Hey there!");

            //create an view
            var view = new CmsPartialView("PartialView", @"~/Views/TestLayOut/TestPartial.cshtml");
            //create a section
            var section = new CmsSection()
            {
                FriendlyName = "DefaultSection",
                ForFlavor = DeviceRenderFlavor.Empty,
                Identifier = "_default"
            };
            //add a slot for the action
            var actionSlot = new CmsSlot(new CmsSectionSlotInfo("_default", true, 15), action);
            section.Slots.Add(actionSlot);
            //add a slot for the view
            var viewSlot = new CmsSlot(new CmsSectionSlotInfo("_default", true, 10), view);
            section.Slots.Add(viewSlot);
            device.Sections.Add(section);
            return device;
        }

        public ActionResult Inner(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) id = "[EMPTY]";
            ViewBag.Id = id;
            return PartialView();
        }
    }
}