using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SingleBlogViewModel
    {
        public string FullText { get; set; }
        public string ImageName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Title { get; set; }
        public string Canonical { get; set; }
        public string MetaDescription { get; set; }
        public string SeoKey { get; set; }
        public string MetaTitle { get; set; }
        public string ImageAlt { get; set; }

    }
}
