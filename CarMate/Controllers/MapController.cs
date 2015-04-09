using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.DAL;
using System.Net;
using CarMate.Classes;
using CarMate.Models;


namespace CarMate.Controllers
{
    public class MapController : Controller
    {
        private CarMateEntities db = new CarMateEntities();
        string formatted_address = "";
        //
        // GET: /Map/
        static double PIx = 3.141592;
        static double RADIO = 6378.16;
        /// Convert degrees to Radians    
        ///  
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
        //запрос к API геокодирования 
        public string GetforrmatedAdress(double lat, double lng)
        {

            string url = string.Format(
                "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=true_or_false&language=ru",
                lat.ToString().Replace(",", "."), lng.ToString().Replace(",", "."));
            //выполняем запрос к универсальному коду ресурса (URI)
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Proxy = MyProxy.CreateProxy();
            request.Method = "GET";
            //получаем ответ от интернет-ресурса
            WebResponse response = request.GetResponse();
            //чтение ресурса
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            //документ xml

            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.LoadXml(responsereader);
            string city = "";
            string region = "";

            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                formatted_address =
                    xmldoc.SelectNodes("//formatted_address").Item(0).InnerText.ToString();
                string[] words = formatted_address.Split(',');
                city = xmldoc.GetElementsByTagName("short_name")[3].ChildNodes[0].InnerText;
                region = xmldoc.GetElementsByTagName("short_name")[2].ChildNodes[0].InnerText;

            }
            return formatted_address;
        }

        private IPlacemarksRepository placemarksRepository;
        private ICategoryRepository categoryRepository;
        private ICountriesRepository countriesRepository;
        private IRegionRepository regionRepository;

        public MapController()
        {
            this.placemarksRepository = new PlacemarksRepository(new CarMateEntities());
            this.categoryRepository = new CategoryRepository(new CarMateEntities());
            this.countriesRepository = new CountriesRepository(new CarMateEntities());
            this.regionRepository = new RegionRepository(new CarMateEntities());
        }

        public MapController(IPlacemarksRepository placemarksRepository,
            ICategoryRepository categoryRepository,
            ICountriesRepository countriesRepository,
            IRegionRepository regionRepository)
        {
            this.placemarksRepository = placemarksRepository;
            this.categoryRepository = categoryRepository;
            this.countriesRepository = countriesRepository;
            this.regionRepository = regionRepository;
        }
        //
        // GET: /Map/Details/5


        public ActionResult GetMap()
        {
            Squirrel s = new Squirrel();
            return View(s);
        }

        [HttpPost]
        public JsonResult PostSquirrel(Squirrel poi)
        {
            List<Squirrel> markers = new List<Squirrel>();
            int categoryId;
            Int32.TryParse(poi.Category, out categoryId);
            double radius;
            Double.TryParse(poi.Radius, out radius);
            var placemarks = from p in placemarksRepository.GetPlacemarks()
                             select p;
            placemarks = placemarks.Where(x => x.categoryId == categoryId).ToList();
            double latitude;
            Double.TryParse(poi.Lat, out latitude);
            double longitude;
            Double.TryParse(poi.Long, out longitude);
            foreach (var p in placemarks)
            {
                if (DistanceBetweenPlaces(p.latitude, p.longitude, latitude, longitude) <= radius)
                {
                    List<double?> prices = db.Prices.Where(x => x.placemarkid == p.id).Select(x => x.price).ToList();//выбор цен по точке
                    List<string> pricesSquirrel = new List<string>();
                    List<string> fuelcatsSquirrel = db.FuelCategories.Where(x => x.countryId == p.countryId).Select(x => x.category).ToList();//выбор категорий по стране
                    foreach (var v in prices)
                    {
                        pricesSquirrel.Add(v.ToString());
                    }
                    formatted_address = GetforrmatedAdress(p.latitude, p.longitude);
                    markers.Add(new Squirrel()
                    {
                        Lat = p.latitude.ToString(),
                        Long = p.longitude.ToString(),
                        Vendore = p.description,
                        Adress = formatted_address,
                        Prices = pricesSquirrel.ToArray(),//формируем ответ по ценам
                        FuelCategories = fuelcatsSquirrel.ToArray()//формируем ответ по категориям 
                    });
                }
            }

            return Json(markers, JsonRequestBehavior.AllowGet);
        }
        //выбор точек по маршруту
        [HttpPost]
        public JsonResult PostlstSquirrel(List<Squirrel> incominglstSquirrel)
        {

            #region получаем категорию и радиус
            Squirrel item = incominglstSquirrel[0];
            int categoryId;
            Int32.TryParse(item.Category, out categoryId);
            double radius;
            Double.TryParse(item.Radius, out radius);
            #endregion

            // выбор из базы данных пои точек по категории и отбор по радиусу

            var placemarks = from p in placemarksRepository.GetPlacemarks()
                             select p;
            placemarks = placemarks.Where(x => x.categoryId == categoryId).ToList();
            List<Squirrel> markers = new List<Squirrel>();//список маркеров по путевым точкам

            foreach (var poisqurrel in incominglstSquirrel)
            {
                double latitude;
                Double.TryParse(poisqurrel.Lat, out latitude);
                double longitude;
                Double.TryParse(poisqurrel.Long, out longitude);
                foreach (var p in placemarks)
                {

                    if (DistanceBetweenPlaces(p.latitude, p.longitude, latitude, longitude) <= radius)
                    {

                        List<double?> prices = db.Prices.Where(x => x.placemarkid == p.id).Select(x => x.price).ToList();
                        List<string> pricesSquirrel = new List<string>();
                        List<string> fuelcatsSquirrel = db.FuelCategories.Where(x => x.countryId == p.countryId).Select(x => x.category).ToList();
                        foreach (var v in prices)
                        {
                            pricesSquirrel.Add(v.ToString());
                        }
                        formatted_address = GetforrmatedAdress(p.latitude, p.longitude);
                        Squirrel temp = new Squirrel()
                        {
                            Lat = p.latitude.ToString(),
                            Long = p.longitude.ToString(),
                            Vendore = p.description,
                            Adress = formatted_address,
                            Prices = pricesSquirrel.ToArray(),
                            FuelCategories = fuelcatsSquirrel.ToArray()
                        };
                        if (!markers.Contains(temp))
                            markers.Add(temp);
                    }

                }
            }
            return Json(markers, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}