using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies;

namespace Ubik.Web.Components.AntiCorruption.Contracts
{
    public interface ITaxonomiesViewModelService
    {
        Task<DivisionViewModel> DivisionModel(int id);

        Task<IPagedResult<DivisionViewModel>> DivisionModels(int pageNumber, int pageSize);

        Task Execute(DivisionSaveModel model);

    }
}