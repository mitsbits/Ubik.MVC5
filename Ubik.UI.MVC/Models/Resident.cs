﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ubik.Web.Cms.Contracts;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Infra.Navigation.Contracts;

namespace Ubik.UI.MVC.Models
{
    public class Resident : IResident
    {
        private readonly IResidentAdministration _administration;
        private readonly IResidentSecurity _security;

        //private readonly IResidentPubSub _pubSub;
        private readonly IModuleDescovery _modules;

        public Resident(IResidentSecurity security, IResidentAdministration administration, IModuleDescovery modules)
        {
            _security = security;
            _administration = administration;
            _modules = modules;
        }

        //public IResidentAdministration Administration
        //{
        //    get { return _administration; }
        //}

        public IResidentSecurity Security
        {
            get { return _security; }
        }

        public IModuleDescovery Modules { get { return _modules; } }

        public IResidentAdministration Administration
        {
            get { return _administration; }
        }

        //public IResidentPubSub PubSub
        //{
        //    get { return _pubSub; }
        //}
    }

    public class ResidentAdministration : IResidentAdministration
    {
        public ResidentAdministration(IMenuProvider<INavigationElements<int>> provider)
        {
            BackofficeMenu = provider;
        }

        public IMenuProvider<INavigationElements<int>> BackofficeMenu { get; private set; }
    }

    public class ModuleDescovery : IModuleDescovery
    {
        private readonly IEnumerable<IModuleDescriptor> _descriptors;

        public ModuleDescovery(IEnumerable<IModuleDescriptor> descriptors)
        {
            _descriptors = descriptors;
        }

        public IReadOnlyCollection<IModuleDescriptor> Installed { get { return new ReadOnlyCollection<IModuleDescriptor>(_descriptors.ToList()); } }
    }
}