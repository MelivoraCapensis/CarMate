using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.Classes
{
    public static class Consts
    {
        public const int StateNew = 0;
        public const int StateUpdate = 1;
        public const int StateDelete = -1;

        // Мойка, нет в списке, ремонт, стоянка, страховка, техосмотр, штраф, эвакуатор
        public const string EventTypeNameAzs = "Заправка";
        public const string EventTypeNameСarwash = "Мойка";
        public const string EventTypeNameOther = "Нет в списке";
        public const string EventTypeNameRepair = "Ремонт";
        public const string EventTypeNameParking = "Стоянка";
        public const string EventTypeNameInsurance = "Страховка";
        public const string EventTypeNameInspection = "Техосмотр";
        public const string EventTypeNamePenalty = "Штраф";
        public const string EventTypeNameEvacuator = "Эвакуатор";
    }
}