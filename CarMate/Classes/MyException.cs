using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.Classes
{
    public class MyException : Exception
    {

        /// <summary>
        /// Получаем текст самого внутреннего исключения
        /// </summary>
        /// <param name="exc"></param>
        /// <returns></returns>
        public static string GetInnerExceptionMessage(Exception exc)
        {
            while (exc.InnerException != null)
            {
                exc = exc.InnerException;
            }
            return exc.Message;
        }

        public static string GetFullExceptionMessage(Exception exc)
        {
            string result = exc.Message;
            while (exc.InnerException != null)
            {
                result += exc.InnerException.Message + ";";
                exc = exc.InnerException;
            }
            return result;
        }
    }
}