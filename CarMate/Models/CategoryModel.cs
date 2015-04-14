using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarMate.Models
{

    public class CategoryModel
    {
        public CategoryModel()
        {}       
        public List<Categories> CategoryList { get; set; }
        public List<int> SelectedCategoryIDs { get; set; }      
    }
}
