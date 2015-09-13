using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Components.Contracts
{
   public interface ICanPublishSuspend
    {
        void Publish();

        void Suspend();

        bool IsPublished { get; }
    }
}
