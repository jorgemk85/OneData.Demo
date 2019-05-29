using OneData.Demo.Models;
using System.Collections.Generic;

namespace OneData.Demo.ViewModels
{
    public class OriginsViewModel : BaseCatalogViewModel
    {
        public List<Origin> Collection { get; set; }
        public Origin Selected { get; set; }
        public bool IsFromCache { get; set; }
    }
}
