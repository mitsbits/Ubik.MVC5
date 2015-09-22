using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Backoffice.Contracts
{
    interface IBackofficeContentProvider
    {
        IBackofficeContent Current { get; }
    }
}
