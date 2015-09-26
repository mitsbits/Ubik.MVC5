using Ubik.Web.Cms.Contracts;
using Ubik.Web.Infra.Navigation.Contracts;

namespace Ubik.UI.MVC.Models
{
    public class Resident : IResident
    {
        private readonly IResidentAdministration _administration;
        private readonly IResidentSecurity _security;
        //private readonly IResidentPubSub _pubSub;

        public Resident(IResidentSecurity security, IResidentAdministration administration)
        {
            _security = security;
            _administration = administration;
        }

        //public IResidentAdministration Administration
        //{
        //    get { return _administration; }
        //}

        public IResidentSecurity Security
        {
            get { return _security; }
        }

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
}