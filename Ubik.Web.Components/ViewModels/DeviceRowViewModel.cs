using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Components.ViewModels
{
   public class DeviceRowViewModel
    {
       public int Id { get; set; }
       public string FriendlyName { get; set; }
       public string Path { get; set; }
       public DeviceRenderFlavor Flavor { get; set; }

    }
}
