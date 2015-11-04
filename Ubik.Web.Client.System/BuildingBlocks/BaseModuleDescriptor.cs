using Ubik.Web.Components;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Client.System.BuildingBlocks
{
    public abstract class BaseModuleDescriptor : IModuleDescriptor
    {
        public abstract string FriendlyName { get; }
        public abstract string Summary { get; }
        public virtual string ModuleGroup { get { return "System"; } }
        public virtual ModuleType ModuleType { get { return ModuleType.Empty; } }

        public abstract BasePartialModule Default();
    }
}