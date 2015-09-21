using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Infra.Navigation.Contracts
{
    public interface IMenuProvider<out TCollection> where TCollection : INavigationElements<int>
    {
        ICollection<NavigationElementDto> Raw { get; }
        TCollection Menu { get; }
    }
}
