using System.Collections.Generic;

namespace Ubik.Web.Components.Contracts
{
    public interface IDevice
    {
        string FriendlyName { get; }

        string Path { get; }

        ICollection<ISection> Sections { get; }

        DeviceRenderFlavor Flavor { get; }
    }

    public interface IContent
    {
        ITextualInfo Textual { get; }
        IHtmlHeadInfo HtmlHeadInfo { get; }
    }

    public interface IHasTags
    {
        IEnumerable<ITag> Tags { get; } 
    }
}