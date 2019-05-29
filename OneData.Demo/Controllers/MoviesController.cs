using Microsoft.AspNetCore.Mvc;
using OneData.Demo.Enums;
using OneData.Demo.Extensions;
using OneData.Demo.Models;
using OneData.Demo.ViewModels;
using OneData.Extensions;
using OneData.Models;
using System;

namespace OneData.Demo.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Set("LastQuery", string.Empty);
            HttpContext.Session.Set("PageOffset", 0);
            MoviesViewModel viewModel = GetNewViewModel(0, DisplayModes.Catalog, null);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult LoadNext()
        {
            int currentPage = HttpContext.Session.Get<int>("PageOffset");
            currentPage++;
            HttpContext.Session.Set("PageOffset", currentPage);
            MoviesViewModel viewModel = GetNewViewModel(currentPage, DisplayModes.Catalog, null);
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
            MoviesViewModel viewModel = GetNewViewModel(currentPage, DisplayModes.Catalog, null);
            return PartialView($"_{viewModel.ControllerName}Table", viewModel);
        }

        private MoviesViewModel GetNewViewModel(int currentPage, DisplayModes displayMode, Guid? searchId)
        {
            MoviesViewModel viewModel = new MoviesViewModel()
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
                    viewModel.Collection = Movie.SelectAll(new QueryOptions() { MaximumResults = 10, Offset = currentPage * 10 });
                }
                else
                {
                    viewModel.Collection = Movie.SelectList(q => q.Name.Contains(lastQuery, StringComparison.InvariantCultureIgnoreCase), new QueryOptions() { MaximumResults = 10, Offset = currentPage * 10 });
                }
                LoadCatalogs(ref viewModel);
            }
            else if (displayMode == DisplayModes.Edit)
            {
                viewModel.Selected = Movie.Select(type => type.Id == searchId.GetValueOrDefault());
                LoadCatalogs(ref viewModel);
            }
            else if (displayMode == DisplayModes.New)
            {
                viewModel.Selected = new Movie();
                LoadCatalogs(ref viewModel);
            }

            return viewModel;
        }

        private void LoadCatalogs(ref MoviesViewModel viewModel)
        {
            viewModel.Origins = Origin.SelectAll();
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
            MoviesViewModel viewModel = GetNewViewModel(0, DisplayModes.New, null);
            return PartialView($"_{viewModel.ControllerName}Edit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            MoviesViewModel viewModel = GetNewViewModel(0, DisplayModes.Edit, id);
            return PartialView($"_{viewModel.ControllerName}Edit", viewModel);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                new Movie(id).Delete();
                MoviesViewModel viewModel = GetNewViewModel(0, DisplayModes.Catalog, null);
                return PartialView($"_{viewModel.ControllerName}Table", viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdited(MoviesViewModel viewModel)
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
        public IActionResult SaveNew(MoviesViewModel viewModel)
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