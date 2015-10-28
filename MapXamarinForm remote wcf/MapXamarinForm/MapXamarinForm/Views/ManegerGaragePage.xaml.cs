using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public partial class ManegerGaragePage : ContentPage
    {
        ManegerGarageViewModel model;
        public ManegerGaragePage(ManegerGarageViewModel model)
        {
            InitializeComponent();
            this.SetValue(Page.TitleProperty, "ManagerGarage");
            this.model = model;
            this.BindingContext = model;
           
            foreach (string fuelcategory in model.FuelcategoryToCar.Keys)
            {
                car_fuelcategory_picker.Items.Add(fuelcategory);
            }
            car_fuelcategory_picker.SelectedIndexChanged += (sender, e) =>
            {
                if (car_fuelcategory_picker.SelectedIndex == -1)
                {
                    model.FuelcategoryId = 1;
                    car_fuelcategory_picker.SelectedIndex = 1;
                }
                else
                {
                    string fuelName = car_fuelcategory_picker.Items[car_fuelcategory_picker.SelectedIndex];
                    model.FuelcategoryId = model.FuelcategoryToCar[fuelName];
                }
            };
            foreach (string type_car in model.TypeToCar.Keys)
            {
                car_type_picker.Items.Add(type_car);
            }
            car_type_picker.SelectedIndexChanged +=async  (sender, e) => 
            {
                if (car_type_picker.SelectedIndex == -1)
                {
                    model.CartypeId = 1;
                    car_type_picker.SelectedIndex = 1;
                }
                else
                {
                    string typeName = car_type_picker.Items[car_type_picker.SelectedIndex];
                    model.CartypeId = model.TypeToCar[typeName];
                }
                await model.InitBrandToCar(model.CartypeId);
                car_brand_picker.Items.Clear();
                foreach (string c_brand in model.BrandToCar.Keys)
                {
                    car_brand_picker.Items.Add(c_brand);
                }
                car_brand_picker.IsEnabled = true;
            };

            
            car_brand_picker.SelectedIndexChanged +=async (sender, e) =>
            {
                if (car_brand_picker.SelectedIndex == -1)
                {
                    model.BrandId= 1;
                    car_brand_picker.SelectedIndex = 1;
                }
                else
                {
                    string brandName = car_brand_picker.Items[car_brand_picker.SelectedIndex];
                    model.BrandId = model.BrandToCar[brandName];
                }
                await model.InitModelToCar(model.BrandId);
                car_model_picker.Items.Clear();
                foreach (string c_model in model.ModelToCar.Keys)
                {
                    car_model_picker.Items.Add(c_model);
                }
                car_model_picker.IsEnabled = true;
            };
            car_model_picker.SelectedIndexChanged +=async (sender, e) => 
            {
                if (car_model_picker.SelectedIndex == -1)
                {
                    model.ModelId = 1;
                    car_model_picker.SelectedIndex = 1;
                }
                else
                {
                    string modelName = car_model_picker.Items[car_model_picker.SelectedIndex];
                    model.ModelId = model.ModelToCar[modelName];
                }
                await model.InitModificationToCar(model.ModelId);
                car_modification_picker.Items.Clear();
                foreach (string c_modification in model.ModificationToCar.Keys)
                {
                    car_modification_picker.Items.Add(c_modification);
                }
                car_modification_picker.IsEnabled = true;
            };
            car_modification_picker.SelectedIndexChanged +=  (sender, e) =>
            {
                if (car_modification_picker.SelectedIndex == -1)
                {
                    model.ModificationId = 1;
                    car_modification_picker.SelectedIndex = 1;
                }
                else
                {
                    string modificationName = car_modification_picker.Items[car_modification_picker.SelectedIndex];
                    model.ModificationId = model.ModificationToCar[modificationName];
                }
            };
            car_odometr.SetBinding(Entry.TextProperty, "CarOdometr");
            car_tank.SetBinding(Entry.TextProperty, "CarTank");
            datebuy_picker.SetBinding(DatePicker.DateProperty, "Datebuy");
            datebuy_picker.Date = DateTime.Now;
            button_add.Clicked += async (sender, e) =>
            {
                if (amountValidator.IsValid && tankamountValidator.IsValid)
                {
                    var task1 = await model.ExecuteSaveCarCommand();
                    await App.UserViewModel.LoadUserCarsAsync();
                    if (task1 == true)
                    {
                        
                        NavigationPage parent = (NavigationPage)this.Parent;
                        RootPage root = (RootPage)parent.Parent;
                        root.Detail = new NavigationPage(new AccountManagerPage(App.UserViewModel));
                    }
                    
                }
                
            };
        }   
    }
}
