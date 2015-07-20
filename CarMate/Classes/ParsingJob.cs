using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using Microsoft.Owin;
using Hangfire.Dashboard;

namespace CarMate.Classes
{
    public class ParsingJob
    {
        public static void ParsePriceFromBrandsAndRegions()
        {
            const string url = @"http://index.minfin.com.ua/fuel/detail.php";
            // Название страны, цены на бензин которой мы парсим.
            //string countryParsing = cbCountryForPrice.SelectedItem.ToString();
            const string countryParsing = Consts.Ukraine;
            const string categoryParsing = Consts.EventTypeNameAzs;
            // Навазвание региона, для которого мы парсим цены (берем из таблицы minfin)
            string regionMinfin = "";
            Regions regionFromDb = null;
            Vendors vendorFromDb = null;

            //try
            //{
                HtmlWeb page = new HtmlWeb
                {
                    // высталяем кодировку
                    OverrideEncoding = System.Text.Encoding.Default,
                    // использовать куки, так будет быстрее
                    UseCookies = true
                };

                //var doc = page.Load(url, "10.3.0.3", 3128, "gevorkyan_ag", "IamGag1963");
                var doc = page.Load(url);
                // Получаем таблицу
                // Ищем таблицу, с атрибутом класс и именем zebra
                var tables = doc.DocumentNode.SelectNodes("//table[@class=\"zebra\"]");
                // Получаем массив строк таблицы
                var tr = tables[0].ChildNodes
                    .Where(x => x.Name == "tr").ToArray();

                using (CarMateEntities db = new CarMateEntities())
                {
                    string fuelname = "";

                    // Нужно получить id Украины из БД
                    var countryFromDb = db.Countries
                        .FirstOrDefault(
                            x => x.Country.Equals(countryParsing /*countryParsing*/, StringComparison.OrdinalIgnoreCase));
                    if (countryFromDb == null)
                    {
                        //MessageBox.Show("В базе данных нет страны \"" + countryParsing + "\"");
                        throw new ArgumentNullException("В базе данных нет страны \"" + countryParsing + "\"");
                        return;
                    }

                    // Для получения categoryId
                    var categoryFromDb = db.Categories
                        .FirstOrDefault(x => x.Category.Equals(categoryParsing, StringComparison.OrdinalIgnoreCase));
                    if (categoryFromDb == null)
                    {
                        //MessageBox.Show("В БД нет категории " + categoryParsing);
                        throw new ArgumentNullException("В БД нет категории " + categoryParsing);
                        return;
                    }


                    // Перебираем массив строк таблицы
                    foreach (var th in tr)
                    {
                        // Перебираем столбцы каждой строки
                        for (int i = 0; i < th.ChildNodes.Count(); i++)
                        {
                            // Если у строки один столбец, то это область
                            if (th.ChildNodes.Count() == 1)
                            {
                                // Вытягиваем область из таблицы и заменяем &nbsp; (неразрывный пробел) на обычный пробел
                                regionMinfin = th.ChildNodes[0].ChildNodes[0].InnerText.Replace("&nbsp;", " ");

                                // Т.к есть цены отдельно для Киева и Киевской области, а нам нужны цены по областям
                                // Убираем город Киев (оставляем Киевскую область)
                                if (regionMinfin.IndexOf("обл", StringComparison.OrdinalIgnoreCase) > 0 ||
                                    regionMinfin.Equals("АР Крым", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Заменяем слово "обл" (Например Запорожкая обл.)
                                    regionMinfin = regionMinfin.Replace(" обл.", " область");
                                    if (regionMinfin.Equals("АР Крым", StringComparison.OrdinalIgnoreCase))
                                        regionMinfin = "Автономная Республика Крым";
                                    // Проверяем, есть ли такой регион в БД
                                    // Нужно получить id области (распарсеной)
                                    regionFromDb = db.Regions
                                        .FirstOrDefault(
                                            x => x.Region.Equals(regionMinfin, StringComparison.OrdinalIgnoreCase));

                                    // Если такой области нет в БД - создаем
                                    if (regionFromDb == null)
                                    {
                                        db.Regions.Add(new Regions
                                        {
                                            CountryId = countryFromDb.Id,
                                            Region = regionMinfin,
                                            DateCreate = DateTime.Now,
                                            State = 0
                                        });
                                        db.SaveChanges();
                                        regionFromDb = db.Regions.FirstOrDefault(x => x.Region.Equals(regionMinfin,
                                            StringComparison.OrdinalIgnoreCase) && x.CountryId == countryFromDb.Id);

                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                // Если ссылка на регион пустая (должны были получить регион, по мере считывания таблицы)
                                if (regionFromDb == null)
                                {
                                    if (regionMinfin.Equals(""))
                                        continue;
                                    //throw new ArgumentNullException("В базе данных нет региона \"" + regionMinfin + "\"");
                                    //MessageBox.Show("В базе данных нет региона \"" + regionMinfin + "\"");
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        // Вытягиваем торговую марку из распарсеной таблицы и если есть ненужные символы заменяем их (неразрывный пробел на обычный пробел)
                                        string vendorMinfin =
                                            th.ChildNodes[i].ChildNodes[0].InnerText.Replace("&nbsp;", " ");
                                        // Проверить существует ли такая торговая марка в заданой стране  и заданой области
                                        vendorFromDb = db.Vendors
                                            .FirstOrDefault(x => x.countryId == countryFromDb.Id &&
                                                                 x.vendor.Equals(vendorMinfin,
                                                                     StringComparison.OrdinalIgnoreCase));

                                        // Если такой торговой марки нет в заданой стране, справшиваем у пользователя, хочет ли он добавить такую торговую марку
                                        //if (vendorFromDb == null)
                                        //{
                                        //    vendorFromDb = db.Vendors.Add(new Vendors
                                        //    {
                                        //        countryId = countryFromDb.id,
                                        //        vendor = vendorMinfin,
                                        //        dateCreate = DateTime.Now,
                                        //        state = 0
                                        //    });
                                        //    db.SaveChanges();
                                        //}
                                    }
                                    // Начиная от 3-го столбца идут цены
                                    else if (i >= 2 && i <= 7)
                                    {
                                        if (i == 2)
                                            fuelname = "A-80";
                                        if (i == 3)
                                            fuelname = "A-92";
                                        if (i == 4)
                                            fuelname = "A-95";
                                        if (i == 5)
                                            fuelname = "A-95p";
                                        if (i == 6)
                                            fuelname = "Diesel";
                                        if (i == 7)
                                            fuelname = "Gas";

                                        // Проверяем, есть ли тип бензина "А80" в заданой стране, в справочнике
                                        FuelCategories fuelCategory = db.FuelCategories
                                            .FirstOrDefault(
                                                x => x.CountryId == countryFromDb.Id && x.Category.Equals(fuelname,
                                                    StringComparison.OrdinalIgnoreCase));

                                        // Если такого вида бензина нет - создаем
                                        if (fuelCategory == null)
                                        {
                                            // res = MessageBox.Show("В базе данных, в стране \"" + country_parsing + "\", нет такого вида топлива \"" + A80_name + "\", хотите добавить?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                            //if (res == System.Windows.Forms.DialogResult.Yes)
                                            //{
                                            fuelCategory = db.FuelCategories.Add(new FuelCategories
                                            {
                                                Category = fuelname,
                                                CountryId = countryFromDb.Id,
                                                DateCreate = DateTime.Now,
                                                State = 0
                                            });
                                            db.SaveChanges();
                                            //}
                                        }

                                        // Получаем все точки, у которые находятся в заданой стране, в заданом регионе, с категорией АЗС и заданой торговой маркой
                                        List<int> lplacemarks = db.Placemarks
                                            .Where(x => x.CountryId == countryFromDb.Id &&
                                                        x.RegionId == regionFromDb.Id &&
                                                        x.CategoryId == categoryFromDb.Id &&
                                                        x.VendorId == vendorFromDb.id)
                                            .Select(x => x.Id)
                                            .ToList();

                                        foreach (var item in lplacemarks)
                                        {
                                            // Ищем цену на конкретную точку, с определенным типом бензина
                                            Prices fuelPrice = db.Prices
                                                .FirstOrDefault(
                                                    x => x.FuelId == fuelCategory.Id && x.PlacemarkId == item);

                                            double fuelPriceMinfin;
                                            string priceMinfin = th.ChildNodes[i].ChildNodes[0].InnerText;
                                            if (!double.TryParse(priceMinfin, out fuelPriceMinfin))
                                            {
                                                // Если не удалось вытянуть, присваиваем 0 (цена не указана)
                                                fuelPriceMinfin = 0;
                                            }

                                            // Если цены на такой вид топлива - создаем
                                            if (fuelPrice == null)
                                            {
                                                if (vendorFromDb != null)
                                                    db.Prices.Add(new Prices
                                                    {
                                                        FuelId = fuelCategory.Id,
                                                        VendorId = vendorFromDb.id,
                                                        RegionId = regionFromDb.Id,
                                                        PlacemarkId = item,
                                                        Price = fuelPriceMinfin,
                                                        UserPrice = 0,
                                                        DateCreate = DateTime.Now
                                                    });
                                            }
                                            else
                                            {
                                                // Изменяем цену на бензин
                                                fuelPrice.Price = fuelPriceMinfin;
                                            }
                                        }
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    db.SaveChanges();
                    //MessageBox.Show("Новые цены успешно добавлены!");
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    MessageBox.Show(ex.StackTrace);
            //}

        }

        public static void ParsePriceFromBrands()
        {
            const string url = @"http://index.minfin.com.ua/fuel/tm.php";
            //url = tBparseUrl.Text.ToString();

            HtmlWeb page = new HtmlWeb
            {
                // высталяем кодировку
                OverrideEncoding = Encoding.Default,
                // использовать куки, так будет быстрее
                UseCookies = true
            };



            //var doc = page.Load(url, "10.3.0.3", 3128, "gevorkyan_ag", "IamGag1963");
            var doc = page.Load(url);
            var tables = doc.DocumentNode.SelectNodes("//table[@class=\"zebra\"]"); // Ищем таблицу, с атрибутом класс и именем zebra
            var tr = tables[0].ChildNodes
                .Where(x => x.Name == "tr").ToArray();
            // int idcountry;
            foreach (var t in tr)
            {
                using (CarMateEntities db = new CarMateEntities())
                {
                    // idcountry = db.Countries.Where(x => x.country.Contains("Украина")).Select(x => x.id).FirstOrDefault();
                    var td = t.ChildNodes
                            .Where(x => x.Name == "td")
                            .ToArray(); // перебираем строки и с каждой строки вытаскиваем цену на бензин

                    if (!td.Any())
                        continue;
                    //block vendor price
                    string vendor = td[0].InnerText;
                    double pricecat1, pricecat2, pricecat3, pricecat4, pricecat5, pricecat6;
                    Double.TryParse(td[1].InnerText, out pricecat1);
                    Double.TryParse(td[2].InnerText, out pricecat2);
                    Double.TryParse(td[3].InnerText, out pricecat3);
                    Double.TryParse(td[4].InnerText, out pricecat4);
                    Double.TryParse(td[5].InnerText, out pricecat5);
                    Double.TryParse(td[6].InnerText, out pricecat6);
                    Vendors ven = db.Vendors.Where(x => x.vendor.Contains(vendor)).Select(x => x).FirstOrDefault();
                    if (ven == null)
                        continue;
                    List<Placemarks> placemarks = db.Placemarks
                        .Include("Vendors")
                        .Where(x => x.Vendors.vendor.Contains(vendor))
                        .Select(x => x).ToList();

                    foreach (var placemark in placemarks)
                    {
                        Prices p1 = db.Prices.Where(x => x.PlacemarkId == placemark.Id && x.FuelId == 1).Select(x => x).FirstOrDefault();
                        if (p1 == null)
                        {

                            p1 = new Prices()
                            {
                                VendorId = (int)placemark.VendorId,
                                FuelId = 1,
                                Price = pricecat1,
                                DateCreate = DateTime.Now,
                                RegionId = (int)placemark.RegionId,
                                PlacemarkId = placemark.Id,
                                State = 1
                            };
                            db.Prices.Add(p1);
                        }
                        else { p1.Price = pricecat1; }

                        Prices p2 = db.Prices.Where(x => x.PlacemarkId == placemark.Id && x.FuelId == 2).Select(x => x).FirstOrDefault();
                        if (p2 == null)
                        {
                            p2 = new Prices()
                            {
                                VendorId = (int)placemark.VendorId,
                                FuelId = 2,
                                Price = pricecat2,
                                DateCreate = DateTime.Now,
                                RegionId = (int)placemark.RegionId,
                                PlacemarkId = placemark.Id,
                                State = 1
                            };
                            db.Prices.Add(p2);
                        }
                        else { p2.Price = pricecat2; }

                        Prices p3 = db.Prices.Where(x => x.PlacemarkId == placemark.Id && x.FuelId == 3).Select(x => x).FirstOrDefault();
                        if (p3 == null)
                        {
                            p3 = new Prices()
                            {
                                VendorId = (int)placemark.VendorId,
                                FuelId = 3,
                                Price = pricecat3,
                                DateCreate = DateTime.Now,
                                RegionId = (int)placemark.RegionId,
                                PlacemarkId = placemark.Id,
                                State = 1
                            };
                            db.Prices.Add(p3);
                        }
                        else { p3.Price = pricecat3; }

                        Prices p4 = db.Prices.Where(x => x.PlacemarkId == placemark.Id && x.FuelId == 4).Select(x => x).FirstOrDefault();
                        if (p4 == null)
                        {
                            p4 = new Prices()
                            {
                                VendorId = (int)placemark.VendorId,
                                FuelId = 4,
                                Price = pricecat4,
                                DateCreate = DateTime.Now,
                                RegionId = (int)placemark.RegionId,
                                PlacemarkId = placemark.Id,
                                State = 1
                            };
                            db.Prices.Add(p4);
                        }
                        else { p4.Price = pricecat4; }

                        Prices p5 = db.Prices.Where(x => x.PlacemarkId == placemark.Id && x.FuelId == 5).Select(x => x).FirstOrDefault();
                        if (p5 == null)
                        {
                            p5 = new Prices()
                            {
                                VendorId = (int)placemark.VendorId,
                                FuelId = 5,
                                Price = pricecat5,
                                DateCreate = DateTime.Now,
                                RegionId = (int)placemark.RegionId,
                                PlacemarkId = placemark.Id,
                                State = 1
                            };
                            db.Prices.Add(p5);
                        }
                        else { p5.Price = pricecat5; }

                        Prices p6 = db.Prices.Where(x => x.PlacemarkId == placemark.Id && x.FuelId == 6).Select(x => x).FirstOrDefault();
                        if (p6 == null)
                        {
                            p6 = new Prices()
                            {
                                VendorId = (int)placemark.VendorId,
                                FuelId = 6,
                                Price = pricecat6,
                                DateCreate = DateTime.Now,
                                RegionId = (int)placemark.RegionId,
                                PlacemarkId = placemark.Id,
                                State = 1
                            };
                            db.Prices.Add(p6);
                        }
                        else { p6.Price = pricecat6; }
                        db.SaveChanges();
                    }
                }
            }

        }
    }

    public class MyRestrictiveAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            // In case you need an OWIN context, use the next line,
            // `OwinContext` class is the part of the `Microsoft.Owin` package.
            var context = new OwinContext(owinEnvironment);

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return context.Authentication.User.Identity.IsAuthenticated;
            if (context.Authentication.User.Identity.IsAuthenticated)
            {
                if (context.Authentication.User.Identity.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    context.Authentication.User.Identity.Name.Equals("AJleksei", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}