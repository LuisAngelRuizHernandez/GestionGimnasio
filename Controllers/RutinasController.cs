using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoGimnasio;
using ProyectoGimnasio.Permisos;

namespace ProyectoGimnasio.Controllers
{

    [ValidarSesion]
    public class RutinasController : Controller
    {
        private GimnasioBDEntities1 db = new GimnasioBDEntities1();

        // GET: Rutinas
        public ActionResult Index()
        {
            int idAprendiz = (int)Session["Admin"];
            var rutinas = db.Rutina.Where(r => r.idAprendiz == idAprendiz).ToList();
            return View(rutinas);
        }

        // GET: Rutinas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutina rutina = db.Rutina.Find(id);
            if (rutina == null)
            {
                return HttpNotFound();
            }
            return View(rutina);
        }

        // GET: Rutinas/Create
        public ActionResult Create()
        {
            // Obtener el idAprendiz de la sesión
            int idAprendiz = (int)Session["idAprendiz"];

            // Asignar el idAprendiz a la vista
            ViewBag.idAprendiz = new SelectList(db.Aprendiz.Where(a => a.idAprendiz == idAprendiz), "idAprendiz", "nombreAprendiz");

            return View();
        }


        // POST: Rutinas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRutina,nombreRutina,idAprendiz")] Rutina rutina)
        {
            if (ModelState.IsValid)
            {
                db.Rutina.Add(rutina);
                    db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idAprendiz = new SelectList(db.Aprendiz, "idAprendiz", "nombreAprendiz", rutina.idAprendiz);
            return View(rutina);
        }

        // GET: Rutinas/Edit/5
        public ActionResult Edit(int? id)
        {
            int idAprendiz = (int)Session["idAprendiz"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutina rutina = db.Rutina.Find(id);
            if (rutina == null)
            { 
                return HttpNotFound();
            }
            ViewBag.idAprendiz = new SelectList(db.Aprendiz.Where(a => a.idAprendiz == idAprendiz), "idAprendiz", "nombreAprendiz", idAprendiz);
            return View(rutina);
        }

        // POST: Rutinas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRutina,nombreRutina,idAprendiz")] Rutina rutina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rutina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idAprendiz = new SelectList(db.Aprendiz, "idAprendiz", "nombreAprendiz", rutina.idAprendiz);
            return View(rutina);
        }

        // GET: Rutinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutina rutina = db.Rutina.Find(id);
            if (rutina == null)
            {
                return HttpNotFound();
            }
            return View(rutina);
        }

        // POST: Rutinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Elimina los detalles de la rutina que tienen la foreign key de la rutina
                    var detallesRutina = db.DetallesRutina.Where(d => d.idRutina == id).ToList();
                    foreach (var detalle in detallesRutina)
                    {
                        db.DetallesRutina.Remove(detalle);
                    }

                    // Elimina la rutina
                    Rutina rutina = db.Rutina.Find(id);
                    db.Rutina.Remove(rutina);

                    // Guarda los cambios en la base de datos
                    db.SaveChanges();

                    // Confirma la transacción
                    transaction.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, revierte la transacción
                    transaction.Rollback();
                    // Maneja el error (podrías registrar el error, mostrar un mensaje, etc.)
                    // Aquí solo se muestra un ejemplo de lanzar la excepción nuevamente
                    throw;
                }
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
