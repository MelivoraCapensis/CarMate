using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace CarMate.Models
{
    public class RegionModel
    {
        public RegionModel()
        {
            RegionList = new List<SelectListItem>();
        }

        [Display(Name = "Region", ResourceType = typeof(Resources.Map))]
        public int Id { set; get; }
        public IEnumerable<SelectListItem> RegionList { set; get; }
    }
}
