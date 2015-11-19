using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Mehdime.Entity;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies;
using Ubik.Web.EF.Components;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.Components.AntiCorruption.Services
{
    public class TaxonomiesViewModelService : ITaxonomiesViewModelService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IPersistedTaxonomyDivisionRepository _divisionRepo;

        private readonly IViewModelBuilder<PersistedTaxonomyDivision, DivisionViewModel> _divisionBuilder;

        private readonly IViewModelCommand<DivisionSaveModel> _divisionCommand;

        public TaxonomiesViewModelService(IDbContextScopeFactory dbContextScopeFactory,
            IPersistedTaxonomyDivisionRepository divisionRepo, IViewModelCommand<DivisionSaveModel> divisionCommand)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _divisionRepo = divisionRepo;
            _divisionCommand = divisionCommand;

            _divisionBuilder = new DivisionViewModelBuilder();
        }

        public async Task<DivisionViewModel> DivisionModel(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                DivisionViewModel model;
                if (id == default(int))
                {
                    model =
                        _divisionBuilder.CreateFrom(new PersistedTaxonomyDivision() {Textual = new PersistedTextual()});
                    _divisionBuilder.Rebuild(model);
                    return await Task.FromResult(model);
                }

                var entity = await _divisionRepo.GetAsync(x => x.Id == id);
                if (entity == null) throw new Exception(string.Format("no taxonomy division with id:{0}", id));
                model = _divisionBuilder.CreateFrom(entity);
                _divisionBuilder.Rebuild(model);
                return model;
            }
        }

        public async Task<IPagedResult<DivisionViewModel>> DivisionModels(int pageNumber, int pageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var source = await _divisionRepo.FindAsync(x => true,
                    new[]
                    {
                        new OrderByInfo<PersistedTaxonomyDivision>()
                        {
                            Ascending = true,
                            Property = m => m.Textual.Subject
                        },
                    },
                    pageNumber, pageSize, m => m.Textual);

                var bucket = new List<DivisionViewModel>();
                foreach (var persistedTaxonomyDivision in source.Data)
                {
                    var model = _divisionBuilder.CreateFrom(persistedTaxonomyDivision);
                    _divisionBuilder.Rebuild(model);
                    bucket.Add(model);
                }
                return new PagedResult<DivisionViewModel>(bucket, source.PageNumber, source.PageSize,
                    source.TotalRecords);
            }
        }

        public async Task Execute(DivisionSaveModel model)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _divisionCommand.Execute(model);
                await db.SaveChangesAsync();
            }

        }
    }
}
