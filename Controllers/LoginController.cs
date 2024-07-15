using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
namespace ProyectoGimnasio.Controllers
{
    public class LoginController : Controller
    {
        static string cadena = "Data Source=DESKTOP-PP002E5;Initial Catalog=GimnasioBD;Integrated Security=True";

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Aprendiz oAprendiz)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("@idAprendiz", oAprendiz.idAprendiz);
                cmd.Parameters.AddWithValue("@claveAprendiz", oAprendiz.claveAprendiz);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int resultado = reader.GetInt32(reader.GetOrdinal("Resultado"));
                        string nombreAprendiz = reader.IsDBNull(reader.GetOrdinal("NombreAprendiz")) ? null : reader.GetString(reader.GetOrdinal("NombreAprendiz"));

                        if (resultado == 1)
                        {
                            Session["idAprendiz"] = oAprendiz.idAprendiz;
                            Session["Admin"] = oAprendiz.idAprendiz;
                            Session["NombreAprendiz"] = nombreAprendiz;
                            return RedirectToAction("InicioAprendiz", "Aprendizs");
                        }
                        else if (resultado == 2)
                        {
                            Session["Admin"] = oAprendiz.idAprendiz;
                            return RedirectToAction("InicioAdministrador", "Administrador");
                        }
                    }
                }
            }

            ViewData["Mensaje"] = "Usuario o clave incorrecta";
            return View();
        }

    }
}