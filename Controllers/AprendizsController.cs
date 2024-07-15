using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoGimnasio;
using System.Data.SqlClient;
using ProyectoGimnasio.Permisos;

namespace ProyectoGimnasio.Controllers
{
    [ValidarSesion]
    public class AprendizsController : Controller
    {
        static string cadena = "Data Source=DESKTOP-PP002E5;Initial Catalog=GimnasioBD;Integrated Security=True";
        int auxx;
        int auxEgreso;
        private GimnasioBDEntities1 db = new GimnasioBDEntities1();
        public ActionResult InicioAprendiz()
        {
            return View();
        }
        

        // GET: Aprendizs
        public ActionResult Index()
        {
            return View(db.Aprendiz.ToList());
        }

        // GET: Aprendizs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aprendiz aprendiz = db.Aprendiz.Find(id);
            if (aprendiz == null)
            {
                return HttpNotFound();
            }
            return View(aprendiz);
        }

        // GET: Aprendizs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Aprendizs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idAprendiz,nombreAprendiz,correoAprendiz,claveAprendiz,fichaAprendiz")] Aprendiz aprendiz)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el ID de aprendiz ya existe en la BD
                var existingAprendiz = db.Aprendiz.FirstOrDefault(a => a.idAprendiz == aprendiz.idAprendiz);

                if (existingAprendiz != null)
                {
                    // Si el ID existe, mostrar el error
                    ModelState.AddModelError("idAprendiz", "El ID de aprendiz ya existe.");
                    return View(aprendiz);
                }

                // Si el ID no existe, se agrega a la BD
                db.Aprendiz.Add(aprendiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aprendiz);
        }

        // GET: Aprendizs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aprendiz aprendiz = db.Aprendiz.Find(id);
            if (aprendiz == null)
            {
                return HttpNotFound();
            }
            return View(aprendiz);
        }

        // POST: Aprendizs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idAprendiz,nombreAprendiz,correoAprendiz,claveAprendiz,fichaAprendiz")] Aprendiz aprendiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aprendiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aprendiz);
        }

        // GET: Aprendizs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aprendiz aprendiz = db.Aprendiz.Find(id);
            if (aprendiz == null)
            {
                return HttpNotFound();
            }
            return View(aprendiz);
        }

        // POST: Aprendizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aprendiz aprendiz = db.Aprendiz.Find(id);
            db.Aprendiz.Remove(aprendiz);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult CerrarSesion()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login", "Login");
        }
        private string GetFecha()
        {
            DateTime fechaHoraActual = DateTime.Now;
            string fechaHoraString = fechaHoraActual.ToString("yyyy-MM-dd HH:mm:ss");
            return fechaHoraString;
        }


        [HttpPost]
        public ActionResult Ingreso(Aprendiz oAprendiz)
        {
            int idAprendiz = (int)Session["idAprendiz"];
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("ValidarRegistro", con);
                cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);
                cmd.Parameters.AddWithValue("@horaIngresada", GetFecha());
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                auxx = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (auxx == 1)
            {
                ViewData["Mensaje1"] = "El aprendiz no existe en la base de datos";
            }
            else
            {
                if (auxx == 2)
                {
                    ViewData["Mensaje1"] = "El aprendiz ya tiene un registro abierto";
                }
                else
                {
                    ViewData["Mensaje1"] = "Ingreso correctamente registrado";
                }
            }
            return View("InicioAprendiz");
        }

        [HttpPost]
        public ActionResult Egreso(Aprendiz oAprendiz)
        {
            int idAprendiz = (int)Session["idAprendiz"];
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("RealizarEgreso", conn);
                cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);
                cmd.Parameters.AddWithValue("@horaSalida", GetFecha());
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                auxEgreso = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (auxEgreso == 3)
            {
                ViewData["Mensaje1"] = "No hay ingreso abierto";
            }
            else
            {
                ViewData["Mensaje1"] = "Egreso realizado correctamente";
            }
            return View("InicioAprendiz");
        }
    }

}
