using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksModels.ViewModels
{
    // We do not need concrete view model for category and cover type
    // because they are standalone in terms of view.
    // Meaning, for the CRUD operations, their views do not require loading other entities.
    // But for Product, we need to load category and cover type into the Upsert view.
    // Hence, this concrete view model encapsulates product, category and cover type for use in that view.
    public class ProductVM
    {
        public Product Product { get; set; }

        // Note: for use of SelectListItem, we need to install Microsoft.AspNetCore.Mvc.ViewFeatures package.
        // And we intend to load the categories as dropdown list.
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
