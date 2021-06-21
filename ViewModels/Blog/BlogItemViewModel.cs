using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlogItemViewModel
    {
        public int BlogID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BlogUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
    }
}
