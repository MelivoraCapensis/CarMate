using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CarMate.Controllers
{
    public class AdminErrorLogsController : BaseAdminController
    {
        //
        // GET: /AdminErrorLogs/

        public ActionResult Index()
        {
            // Получаем корневой каталог
            string rootCatalog = Server.MapPath("~");
            // Получаем все папки (с путями), которые находятся в каталоге "Content\logs"
            string[] paths = Directory.GetDirectories(rootCatalog + @"Content\logs");
            // Получаем названия папок (обрезаем путя)
            List<string> directories = paths.Select(Path.GetFileName).ToList();

            return View(directories);
        }

        public ActionResult LogsByDate(string directory)
        {
            // Получаем корневой каталог
            string rootCatalog = Server.MapPath("~");
            // Проверяем существует ли такой каталог
            if (Directory.Exists(rootCatalog + @"Content\logs\" + directory))
            {
                // Получаем все файлы, которые находятся в указанном каталоге
                string[] files = Directory.GetFiles(rootCatalog + @"Content\logs\" + directory);
                // Получаем имена файлов (обрезаем путя)
                List<string> filesName = files.Select(Path.GetFileName).ToList();
                ViewBag.Directory = directory + @"\";
                //filesName.Sort();
                //filesName.Reverse();

                return View(filesName);
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult OpenLog(string fileName)
        {
            // Получаем корневой каталог
            string rootCatalog = Server.MapPath("~");
            string filePath = rootCatalog + @"Content\logs\" + fileName;
            // Проверяем существует ли такой файл
            if (System.IO.File.Exists(filePath))
            {
                //********************
                string allText = System.IO.File.ReadAllText(filePath, Encoding.Default);
                List<string> messages = allText
                    .Split(new[] {"********************\r\n"}, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                List<Log> logs = new List<Log>();
                foreach (var message in messages)
                {
                    if (message.Length >= 24)
                        logs.Add(new Log(message));
                }
                ViewBag.Directory = fileName.Split('\\')[0];

                return View(logs);
            }
            return RedirectToAction("Index");
        }

    }
}


public class Log
{
    public Log(string log)
    {
        CultureInfo ci = new CultureInfo("ru");
        string startDate = log.Substring(0, 24);
        DateTime date;
        if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.ffff", ci, DateTimeStyles.None, out date))
        {
            DateError = date;
        }
        else
        {
            DateError = DateTime.MinValue;
        }
        Message = log.Substring(24);
    }

    public DateTime DateError { set; get; }
    public string Message { set; get; }
}