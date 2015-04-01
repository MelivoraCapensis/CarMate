using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.Models
{
    public class VendorModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public List<string> Categories { set; get; }
        public List<double> Prices { set; get; }
    }
}