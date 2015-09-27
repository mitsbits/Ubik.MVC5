namespace Ubik.Web.Auth.Contracts
{
    public interface IViewModelCommand<in TViewModel>
    {
        void Execute(TViewModel model);
    }
}