using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoGimnasio.Permisos;
using ClassLibrary1;


namespace ProyectoGimnasio.Controllers
{
    [ValidarSesion]
    public class AdministradorController : Controller
    {
        // GET: Administrador
        public ActionResult InicioAdministrador()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login", "Login");
        }
    }
}