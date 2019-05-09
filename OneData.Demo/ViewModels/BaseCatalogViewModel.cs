using OneData.Demo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneData.Demo.ViewModels
{
    public class BaseCatalogViewModel
    {
        public string ControllerName { get; set; }
        public DisplayModes DisplayMode { get; set; }
        public int CurrentPage { get; set; }
        public string SearchQuery { get; set; }
        public string SearchPlaceHolder { get; set; }
    }
}
