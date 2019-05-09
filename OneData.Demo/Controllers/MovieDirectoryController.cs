using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneData.Demo.Models;
using OneData.Demo.ViewModels;

namespace OneData.Demo.Controllers
{
    public class MovieDirectoryController : Controller
    {
        public IActionResult Index()
        {
            MovieDirectoryViewModel viewModel = new MovieDirectoryViewModel()
            {
                MaxItemsPerLine = 5,
                Movies = Movie.SelectAll()
            };
            return View(viewModel);
        }
    }
}