using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.ViewModel
{
    public class CarConsumptionViewModel
    {
        // Первая заправка полным баком
        public CarEvents OldCarEvent { set; get; }
        // Вторая заправка полным баком
        public CarEvents NewCarEvent { set; get; }
        // Расход между первой и второй заправкой с полным баком
        public double Consumption { set; get; }
        // Пробег между первой и второй заправкой с полным баком
        public int Mileage { set; get; }

        public CarConsumptionViewModel(CarEvents oldCarEvent, CarEvents newCarEvent)
        {
            OldCarEvent = oldCarEvent;
            NewCarEvent = newCarEvent;
            Mileage = newCarEvent.Odometer - oldCarEvent.Odometer;
            if (NewCarEvent.FuelCount != null)
                Consumption = (double)NewCarEvent.FuelCount * 100 / Mileage;
            else
                Consumption = 0;
        }

    }
}