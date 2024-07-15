using System;
using System.Web;
using System.Web.Mvc;

namespace ProyectoGimnasio.Permisos
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Admin"] == null)
            {
                System.Diagnostics.Debug.WriteLine("Sesion Admin no encontrada, redirigiendo a Login.");
                filterContext.Result = new RedirectResult("~/Login/Login");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Sesion Admin encontrada, continuando.");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
