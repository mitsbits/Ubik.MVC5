﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Ubik.Infra;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Backoffice.Controllers
{
    public class TaxonomiesController : BackofficeController
    {
        private readonly ITaxonomiesViewModelService _viewModelService;

        public TaxonomiesController(IErrorLogManager errorLogManager, ITaxonomiesViewModelService viewModelService) : base(errorLogManager)
        {
            _viewModelService = viewModelService;
        }

        public async Task<ActionResult> Divisions(int? id)
        {
            try
            {
                return !id.HasValue
                 ? View(await _viewModelService.DivisionModels(Pager.Current, Pager.RowCount))
                 : View(await _viewModelService.DivisionModel(id.Value));
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToRoute(new {Controller = "Home", Action = "Index"});
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateDivision(DivisionViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                }
                await _viewModelService.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("Division '{0}' saved!", model.Name));
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
            }
            return RedirectToAction("divisions", "taxonomies", null);
        }
    }
}