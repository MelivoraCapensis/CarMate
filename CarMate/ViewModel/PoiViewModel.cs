using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarMate.Models;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;

namespace CarMate.ViewModel
{
    public class Info
    {
        public int Id { set; get; }
        public double Lat { set; get; }
        public double Long { set; get; }
        public List<double?> Pr { set; get; }
        public string Vendore { set; get; }

    }
    public class PoiViewModel
    {
        public List<Placemarks> Points { set; get; }//коллекция точек
        public Placemarks SelectedPoint { set; get; }//коллекция цен
        public List<Info> Prices { set; get; }
        public POI geoPoi { set; get; }
        public PoiViewModel()
        {
            Points = new List<Placemarks>();
            Prices = new List<Info>();
            geoPoi = new POI();
        }
        public double Latitude { set; get; }
        public double Longitude { set; get; }
    }
   
    public class Location
    {
        public string FuelCategory { get; set; }
        public double? UserPrice { get; set; }
        public double? Price { get; set; }
    }
    public class POI
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string description { get; set; }
    }
  
}
