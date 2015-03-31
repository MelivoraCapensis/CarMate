using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CarMate.Models
{
    public class CountryModel
    {
        public CountryModel()
        {
            CountryList = new List<SelectListItem>();
        }
        [Display(Name = "Страна")]
        public int Id { get; set; }
        public IEnumerable<SelectListItem> CountryList { set; get; }
    }
}