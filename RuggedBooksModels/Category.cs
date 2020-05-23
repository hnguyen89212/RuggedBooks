using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RuggedBooksModels
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        // The Display annotation will replace asp-for in Razor page with the Name's value (Category Name in this case).
        [Display(Name="Category Name")]
        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }
    }
}
