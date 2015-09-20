using System.Collections;
using System.Collections.Generic;
using Ubik.Infra.Contracts;
using Ubik.Web.Infra.Navigation.Contracts;

namespace Ubik.Web.Infra.Navigation
{
    public class BaseNavigationElements<TData> : INavigationElements<int> where TData : class, INavigationElement<int>
    {
        protected readonly IEnumerable<TData> Descedants;

        public BaseNavigationElements(IEnumerable<TData> descedants)
        {
            Descedants = descedants;
        }

        public IEnumerator GetEnumerator()
        {
            if (Descedants == null) return null;

            var enumurator = Descedants.GetEnumerator();
            return enumurator;
        }

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }
    }
}
