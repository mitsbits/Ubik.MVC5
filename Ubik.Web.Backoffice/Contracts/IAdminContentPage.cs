using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.BackOffice.Contracts
{
    public interface IBackofficeContent
    {
        string Title { get; }
    }

    class BackofficeContent : IBackofficeContent
    {
       public string Title { get; set; }
    }
}
