using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Components.Contracts
{
    interface IPartialViewComponent
    {
        string ClassName { get; }

        string TypeFullName { get; }

        Object[] InvokeParameters { get; }
    }
}
