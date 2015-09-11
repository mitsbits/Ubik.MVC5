using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Components.Contracts
{
    interface ICatalogued
    {
        string Key { get; }

        string HumanKey { get; }

        string Value { get; }

        string Hint { get; }

        string Flag { get; }
    }
}
