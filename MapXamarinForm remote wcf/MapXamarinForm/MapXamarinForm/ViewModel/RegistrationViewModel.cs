using MapXamarinForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapXamarinForm.ViewModel
{
    class RegistrationViewModel:INotifyPropertyChanged
    {
        string firstname, lastname, nickname, password, dateRegister,useremail;
        string model, modification, odometr, tank, datebuy, fuelcategory,brand;
        public event PropertyChangedEventHandler PropertyChanged;
        public string FirstName
        {
            get { return firstname; }
            set
            {
                if (firstname != value)
                {
                    firstname = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                    }
                }
            }
        }
        public string LastName
        {
            get { return lastname; }
            set
            {
                if (lastname != value)
                {
                    lastname = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                    }
                }
            }
        }
        public string UserEmail
        {
            get { return useremail; }
            set
            {
                if (useremail != value)
                {
                    useremail = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("UserEmail"));
                    }
                }
            }
        }
        public string DateRegister
        {
            get { return dateRegister; }
            set
            {
                if (dateRegister != value)
                {
                    dateRegister = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateRegister"));
                    }
                }
            }
        }
        public string Nickname
        {
            get { return nickname; }
            set
            {
                if (nickname != value)
                {
                    nickname = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Nickname"));
                    }
                }
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                    }
                }
            }
        }
        public string Brand
        {
            get { return brand; }
            set
            {
                if (brand != value)
                {
                    brand = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Brand"));
                    }
                }
            }
        }
        public string Model
        {
            get { return model; }
            set
            {
                if (model != value)
                {
                    model = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Model"));
                    }
                }
            }
        }
        public string Modification
        {
            get { return modification; }
            set
            {
                if (modification != value)
                {
                    modification = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Modification"));
                    }
                }
            }
        }
        public string Odometr
        {
            get { return odometr; }
            set
            {
                if (odometr != value)
                {
                    odometr = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Odometr"));
                    }
                }
            }
        }
        public string Tank
        {
            get { return tank; }
            set
            {
                if (tank != value)
                {
                    tank = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Tank"));
                    }
                }
            }
        }
        public string Datebuy
        {
            get { return datebuy; }
            set
            {
                if (datebuy != value)
                {
                    datebuy = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Datebuy"));
                    }
                }
            }
        }
        public string Fuelcategory
        {
            get { return fuelcategory; }
            set
            {
                if (fuelcategory != value)
                {
                    fuelcategory = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Fuelcategory"));
                    }
                }
            }
        }
        public async Task<bool> IsExistsUserAsync()
        {
            var task1=App.Database.IsExistsUserAsync();
            var res1 = await task1;
            if (res1 == true)
                return true;
            else
                return false;
        }
        public async Task<bool> LaodUserDataAsync()
        {
   
            var task1 = App.Database.GetUserWithChildren();
            var res1 = await task1;
            await Task.Delay(500);

          
                     
            this.LastName = res1.LastName;
            this.FirstName = res1.FirstName;
            this.UserEmail = res1.UserEmail;
            this.Nickname = res1.Nickname;
            this.Password = res1.UserPassword;
            this.DateRegister = res1.DateRegister.ToString();
            Car firstCar;//null
            var task6 = App.Database.GetCarByServUserId(res1.UserId);
            var res6 = await task6;
            firstCar = res6[0];
            var task2 = App.Database.GetCarModel(firstCar.ServModelId);
            var res2 = await task2;
            this.Model = res2.model;

            var task5 = App.Database.GetCarBrand(res2.ServBrandId);
            var res5 = await task5;
            this.Brand = res5.Brand;

            var task3 = App.Database.GetCarModification(firstCar.ServModificationId);
            var res3 = await task3;
            this.Modification = res3.Modification;

            var task4 = App.Database.GetFuelCategory(firstCar.ServFuelCategoryId);
            var res4 = await task4;
            this.Fuelcategory = res4.FuelCategoryName;
            this.Odometr = firstCar.Odometer.ToString();
            this.Tank = firstCar.Tank.ToString();
            this.Datebuy = firstCar.DateBuy.ToString();

            return true;
        }
        public async Task UpdateUserAsync()
        {
            User updateUser = new User();
            updateUser.FirstName = this.FirstName;
            updateUser.LastName = this.LastName;
            updateUser.Nickname = this.Nickname;
            updateUser.UserPassword = this.Password;
            updateUser.UserEmail = this.UserEmail;
            var task1 = App.Database.UpdateUserAsync(updateUser);
            var res1 = await task1;

        }
    }
}
