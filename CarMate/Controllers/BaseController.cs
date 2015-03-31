using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarMate.Controllers
{
    public class BaseController : Controller
    {
        protected CarMateEntities db = new CarMateEntities();
        protected int UserId;

        #region С хабра методы, для перенаправления на страницы

        protected static string ErrorPage = "~/Error";
        protected static string NotFoundPage = "~/NotFoundPage";
        protected static string LoginPage = "~/Login";

        public RedirectResult RedirectToNotFoundPage
        {
            get { return Redirect(NotFoundPage); }
        }

        public RedirectResult RedirectToLoginPage
        {
            get { return Redirect(LoginPage); }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            filterContext.Result = Redirect(ErrorPage);
        }
        #endregion
    }
}
