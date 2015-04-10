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
        //// Минимальный расход бензина
        //public static double MinConsumption { set; get; }
        //// Сумма расхода бензина
        //public static double SummConsumption { set; get; }
        //// Максимальный расход бензина
        //public static double MaxConsumption { set; get; }

        public CarConsumptionViewModel(CarEvents oldCarEvent, CarEvents newCarEvent, double consumption, int mileage)
        {
            OldCarEvent = oldCarEvent;
            NewCarEvent = newCarEvent;
            Mileage = newCarEvent.Odometer - oldCarEvent.Odometer;
            Consumption = consumption;
        }

    }
}