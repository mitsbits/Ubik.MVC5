using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Components.Contracts
{
    interface ITaxonomyElement<out TKey> : IEntity<TKey>
    {
        TKey ParentId { get; }
        int Depth { get; }
    }
}
