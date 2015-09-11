﻿using System.Collections.Generic;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Cms.Contracts
{
    internal interface IDevicePageProvider
    {
        IDevice Current { get; }

        IEnumerable<ISection> ActiveSections { get; }
    }
}