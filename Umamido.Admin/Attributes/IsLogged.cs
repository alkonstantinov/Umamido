using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Umamido.Admin.Attributes
{
    /// <summary>
    /// Филтър проверяващ дали потребителят е логнат в системата
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IsLogged : AuthorizeAttribute
    {

        /// <summary>
        /// Конструктор на класа
        /// </summary>
        public IsLogged()
        {

        }


        /// <summary>
        /// Проверява дали потребителят е логнат в системата
        /// </summary>
        /// <param name="httpContext">Контекст на проверката</param>
        /// <returns>резултат от проверката</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return (httpContext.Session["UserId"] != null);
        }

    }
}