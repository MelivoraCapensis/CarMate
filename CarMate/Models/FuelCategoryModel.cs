using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace CarMate.Models
{
    public class FuelCategoryModel
    {
        public FuelCategoryModel()
        { }
        public List<FuelCategories> FuelCategoryList { get; set; }
        public List<int> SelectedFuelCategoryIDs { get; set; }   
    }
    
}