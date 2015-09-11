using System.Web.Mvc;

namespace Ubik.Web.Cms
{
    public abstract class CmsPage<TModel> : WebViewPage<TModel>
    {
        public DeviceHelper Device { get; private set; }

        //public ContentHelper Content { get; private set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            Device = new DeviceHelper(ViewContext);
            //Content = new ContentHelper(ViewContext);
        }
    }

    public abstract class CmsPage : WebViewPage
    {
        public DeviceHelper Device { get; private set; }

        //public ContentHelper Content { get; private set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            Device = new DeviceHelper(ViewContext);
            //Content = new ContentHelper(ViewContext);
        }
    }
}