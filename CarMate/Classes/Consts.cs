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

        public const string EventTypeNameAzs = "Заправка";
    }
}