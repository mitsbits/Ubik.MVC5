namespace Ubik.Web.Auth.Contracts
{
    internal interface IViewModelCommand<in TViewModel>
    {
        void Execute(TViewModel model);
    }
}