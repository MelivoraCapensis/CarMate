using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace CarMate.Classes
{
    public static class MyProxy
    {
        // Просто создает прокси
        public static WebProxy CreateProxy()
        {
            WebProxy proxy = new WebProxy("10.3.0.3", 3128)
            {
                Credentials = new NetworkCredential("gevorkyan_ag", "IamGag1963")
            };
            return proxy;
        }
    }
}