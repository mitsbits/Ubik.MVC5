using System.Threading.Tasks;

namespace Ubik.Web.Auth.Contracts
{
    public interface IViewModelCommand<in TViewModel>
    {
        Task Execute(TViewModel model);
    }
}