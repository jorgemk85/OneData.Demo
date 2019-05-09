using OneData.Demo.Models;
using System.Collections.Generic;

namespace OneData.Demo.ViewModels
{
    public class MoviesViewModel : BaseCatalogViewModel
    {
        public List<Movie> Collection { get; set; }
        public IEnumerable<Origin> Origins { get; set; }
        public Movie Selected { get; set; }
    }
}
