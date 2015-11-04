using System;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.Ext;
using Ubik.Web.EF.Components;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies
{
    public class DivisionSaveModel
    {
        public int Id { get; set; }
        public int TextualId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
    }

    public class DivisionViewModel : DivisionSaveModel
    {
    }

    public class DivisionViewModelBuilder : IViewModelBuilder<PersistedTaxonomyDivision, DivisionViewModel>
    {
        public DivisionViewModel CreateFrom(PersistedTaxonomyDivision entity)
        {
            return new DivisionViewModel() { Id = entity.Id, Name = entity.Textual.Subject, Summary = entity.Textual.Summary.ToUTF8() };
        }

        public void Rebuild(DivisionViewModel model)
        {
            return;
        }
    }

    public class DivisionViewModelCommand : IViewModelCommand<DivisionSaveModel>
    {
        private readonly IPersistedTaxonomyDivisionRepository _repo;

        public DivisionViewModelCommand(IPersistedTaxonomyDivisionRepository repo)
        {
            _repo = repo;
        }

        public Task Execute(DivisionSaveModel model)
        {
            throw new NotImplementedException();
        }
    }
}