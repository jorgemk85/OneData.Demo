﻿using Microsoft.AspNetCore.Mvc;
using OneData.DAO;
using OneData.Demo.Enums;
using OneData.Demo.Extensions;
using OneData.Demo.Models;
using OneData.Demo.ViewModels;
using OneData.Extensions;
using OneData.Models;
using System;

namespace OneData.Demo.Controllers
{
    public class OriginsController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Set("LastQuery", string.Empty);
            HttpContext.Session.Set("PageOffset", 0);
            OriginsViewModel viewModel = GetNewViewModel(0, DisplayModes.Catalog, null);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult LoadNext()
        {
            int currentPage = HttpContext.Session.Get<int>("PageOffset");
            currentPage++;
            HttpContext.Session.Set("PageOffset", currentPage);
            OriginsViewModel viewModel = GetNewViewModel(currentPage, DisplayModes.Catalog, null);
            return PartialView($"_{viewModel.ControllerName}Table", viewModel);
        }

        [HttpGet]
        public IActionResult LoadPrevious()
        {
            int currentPage = HttpContext.Session.Get<int>("PageOffset");
            if (currentPage > 0)
            {
                currentPage--;
            }

            HttpContext.Session.Set("PageOffset", currentPage);
            OriginsViewModel viewModel = GetNewViewModel(currentPage, DisplayModes.Catalog, null);
            return PartialView($"_{viewModel.ControllerName}Table", viewModel);
        }

        private OriginsViewModel GetNewViewModel(int currentPage, DisplayModes displayMode, Guid? searchId)
        {
            Result<Origin> result = null;
            QueryOptions queryOptions = new QueryOptions() { MaximumResults = 10, Offset = currentPage * 10 };
            OriginsViewModel viewModel = new OriginsViewModel()
            {
                CurrentPage = currentPage + 1,
                ControllerName = RouteData.Values["controller"].ToString(),
                DisplayMode = displayMode
            };

            if (displayMode == DisplayModes.Catalog || displayMode == DisplayModes.Search)
            {
                string lastQuery = HttpContext.Session.Get<string>("LastQuery");
                if (string.IsNullOrWhiteSpace(lastQuery))
                {
                    result = Manager<Origin>.SelectAll(queryOptions);
                    viewModel.IsFromCache = result.IsFromCache;
                    viewModel.Collection = result.Data.ToList();
                }
                else
                {
                    result = Manager<Origin>.Select(q => q.Name.Contains(lastQuery, StringComparison.InvariantCultureIgnoreCase), queryOptions);
                    viewModel.IsFromCache = result.IsFromCache;
                    viewModel.Collection = result.Data.ToList();
                }
            }
            else if (displayMode == DisplayModes.Edit)
            {
                viewModel.Selected = Origin.Select(type => type.Id == searchId.GetValueOrDefault());
            }
            else if (displayMode == DisplayModes.New)
            {
                viewModel.Selected = new Origin();
            }

            return viewModel;
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return RedirectToAction("Index");
            }
            else
            {
                HttpContext.Session.Set("LastQuery", searchQuery);
                return View("Index", GetNewViewModel(0, DisplayModes.Search, null));
            }
        }

        [HttpGet]
        public IActionResult New()
        {
            OriginsViewModel viewModel = GetNewViewModel(0, DisplayModes.New, null);
            return PartialView($"_{viewModel.ControllerName}Edit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            OriginsViewModel viewModel = GetNewViewModel(0, DisplayModes.Edit, id);
            return PartialView($"_{viewModel.ControllerName}Edit", viewModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                new Origin(id).Delete();
                OriginsViewModel viewModel = GetNewViewModel(0, DisplayModes.Catalog, null);
                return PartialView($"_{viewModel.ControllerName}Table", viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdited(OriginsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.Selected.Update();
                    viewModel = GetNewViewModel(0, DisplayModes.Catalog, null);
                    return PartialView($"_{viewModel.ControllerName}Table", viewModel);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Please make sure you filled every field properly.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveNew(OriginsViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.Selected != null)
            {
                try
                {
                    viewModel.Selected.Insert();
                    viewModel = GetNewViewModel(0, DisplayModes.Catalog, null);
                    return PartialView($"_{viewModel.ControllerName}Table", viewModel);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Please make sure you filled every field properly.");
            }
        }
    }
}