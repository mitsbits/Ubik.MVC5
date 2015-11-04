﻿using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Contracts;
using Ubik.Web.Infra.Navigation;

namespace Ubik.Web.Backoffice
{
    public class BackofficeNavigationElement : BaseNavigationElement
    {
        public BackofficeNavigationElement(IEnumerable<NavigationElementDto> data, int id)
            : base(data, id)
        {
        }

        public override IHierarchicalEnumerable GetChildren()
        {
            var result = new BackofficeNavigationElements(
                  Data.Where(x => x.ParentId == Proxy.Id)
                  .OrderBy(x => x.Weight)
                  .Select(x => new BackofficeNavigationElement(Data, x.Id))
                  .ToList());
            return result;
        }

        public override IHierarchyData GetParent()
        {
            return Data.Any(x => x.Id == Proxy.ParentId)
                ? new BackofficeNavigationElement(Data, Proxy.ParentId)
                : null;
        }
    }

    public class BackofficeNavigationElements : BaseNavigationElements<BackofficeNavigationElement>
    {
        public BackofficeNavigationElements(IEnumerable<BackofficeNavigationElement> descedants)
            : base(descedants)
        {
        }
    }
}