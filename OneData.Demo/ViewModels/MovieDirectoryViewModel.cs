using OneData.Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneData.Demo.ViewModels
{
    public class MovieDirectoryViewModel
    {
        public List<Movie> Movies { get; set; }
        public int MaxItemsPerLine { get; set; }
    }
}
