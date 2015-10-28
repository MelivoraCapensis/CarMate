using MapXamarinForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapXamarinForm.Service
{
    public class PoiService
    {
        static double PIx = 3.141592;
        static double RADIO = 6378.16;
      
        public PoiService()
        {
        }
        public static double Radians(double x)
        {
            return x * PIx / 180;
        }
        /// Calculate the distance between two places.
        public static double DistanceBetweenPlaces(
           double lon1,
           double lat1,
           double lon2,
           double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (angle * RADIO);//distance in miles
        }

        public async Task<List<Poi>> GetPoiForCoordinates(Double latitude, Double longitude,double radius ,int servFuelId,List<int>fuel_categoryId)
        {
            List<Placemark> lplacemark = await App.Database.GetPlacemark();
            List<Poi> lpoi = new List<Poi>();
            if (lplacemark != null && fuel_categoryId.Count==0)
            {
                foreach (var p in lplacemark)
                {
                    if (DistanceBetweenPlaces((double)p.Latitude, (double)p.Longitude, latitude, longitude) <= radius)
                    {
                        List<Price> lprice = await App.Database.GetPriceByPlacemark(p.PlacemarkId);
                        FuelCategory fcategory = await App.Database.GetFuelCategory(servFuelId);
                        Poi temp = new Poi();
                        temp.geometry = new Geometry();
                        temp.geometry.location = new Location();
                        temp.geometry.location.lat =(double) p.Latitude;
                        temp.geometry.location.lng =(double) p.Longitude;
                        temp.vendor = p.Description;
                        temp.prices = new List<double>();
                        temp.fuelcetegory = new List<string>();
                        if (lprice != null)
                        {
                            foreach (var pr in lprice)
                            {
                                if (pr.ServFuelId == servFuelId)
                                {
                                    temp.fuelcetegory.Add(fcategory.FuelCategoryName);
                                    temp.prices.Add((double)pr.PriceCost);
                                }

                                    
                            }                          
                        }
                        lpoi.Add(temp);
                    }
                }
            }
            else if (lplacemark != null && fuel_categoryId.Count > 0)
            {
                foreach (var p in lplacemark)
                {
                    if (DistanceBetweenPlaces((double)p.Latitude, (double)p.Longitude, latitude, longitude) <= radius)
                    {                        
                        Poi temp = new Poi();
                        temp.geometry = new Geometry();
                        temp.geometry.location = new Location();
                        temp.geometry.location.lat = (double)p.Latitude;
                        temp.geometry.location.lng = (double)p.Longitude;
                        temp.vendor = p.Description;
                        temp.prices = new List<double>();
                        temp.fuelcetegory = new List<string>();
                        List<Price> lprice = await App.Database.GetPriceByPlacemark(p.PlacemarkId);
                        for (int i = 0; i < fuel_categoryId.Count; i++)
                        {
                            int fuelcategoryId_temp = fuel_categoryId[i];
                            FuelCategory fcategory = await App.Database.GetFuelCategory(fuelcategoryId_temp);                           
                            if (lprice != null)
                            {
                                foreach (var pr in lprice)
                                {
                                    if (pr.ServFuelId == fuelcategoryId_temp)
                                    {
                                        temp.fuelcetegory.Add(fcategory.FuelCategoryName);
                                        temp.prices.Add((double)pr.PriceCost);
                                    }
                                }
                            }
                            lpoi.Add(temp);
                        }                       
                    }
                }
            }
            return lpoi;
        }
    }
}
