 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace ProyectoGimnasio.Controllers
{

    public class IngresoController : Controller
    {
        static string cadena = "Data Source=DESKTOP-PP002E5;Initial Catalog=GimnasioBD;Integrated Security=True";
        int aux;

        // GET: Ingreso
        public ActionResult Index()
        {
            return View();
        }

        private string GetFecha()
        {
            DateTime fechaHoraActual = DateTime.Now;
            string fechaHoraString = fechaHoraActual.ToString("yyyy-MM-dd HH:mm:ss");
            return fechaHoraString;
        }

         public ActionResult reporteSimple()

        {
            return View();
        }
















        [HttpPost]
        public ActionResult IngresoProceso(Aprendiz oAprendiz)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("ValidarRegistro", cn);
                cmd.Parameters.AddWithValue("@idAprendiz", oAprendiz.idAprendiz);
                cmd.Parameters.AddWithValue("@horaIngresada", GetFecha());
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                
                aux = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (aux == 1)
            {
                ViewData["Mensaje1"] = "El aprendiz no existe en la base de datos";
                return View();
            }
            else
            {
                if (aux == 2)
                {
                    ViewData["Mensaje1"] = "El aprendiz ya tiene un registro abierto";
                    return View();

                }
                else
                {
                    ViewData["Mensaje1"] = "Ingreso correctamente registrado";
                    return View();
                }
            }
        }
    }
}