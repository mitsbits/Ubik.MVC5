using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubik.Web.Infra.Navigation;

namespace Ubik.Web.Backoffice
{
  public  class BackofficeNavigationElement : BaseNavigationElement
    {
      public BackofficeNavigationElement(IEnumerable<NavigationElementDto> data, int id) : base(data, id)
      {
      }
    }

  public class BackofficeNavigationElements : BaseNavigationElements<BackofficeNavigationElement>
    {
      public BackofficeNavigationElements(IEnumerable<BackofficeNavigationElement> descedants) : base(descedants)
      {
      }
    }
}
