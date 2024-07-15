    using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoGimnasio;
using ProyectoGimnasio.Controllers;
using ProyectoGimnasio.Permisos;

namespace ProyectoGimnasio.Controllers
{
    [ValidarSesion]
    public class DetallesRutinasController : Controller
    {
        private GimnasioBDEntities1 db = new GimnasioBDEntities1();

        // GET: DetallesRutinas
        //ESTO ES LO QUE HAY QUE CAMBIARRRRRRRRRRRRRR--------------------------

        // GET: DetallesRutinas
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                // Handle the case when no id is provided, e.g., show all routines or return an error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Obtener los detalles de la rutina específica
            var detallesRutina = db.DetallesRutina
                                   .Where(d => d.idRutina == id)
                                   .Include(d => d.Ejercicios)
                                   .Include(d => d.Rutina); // Incluir la información de los ejercicios y rutinas relacionados

            ViewBag.idRutina = id;          

            return View(detallesRutina.ToList());
        }


        // GET: DetallesRutinas/Details/5
        // GET: DetallesRutinas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DetallesRutina detallesRutina = db.DetallesRutina
                                              .Include(d => d.Ejercicios)
                                              .Include(d => d.Rutina)
                                              .FirstOrDefault(d => d.idDetalle == id);

            if (detallesRutina == null)
            {
                return HttpNotFound();
            }

            ViewBag.idRutina = detallesRutina.idRutina; // Store the current rutina id in ViewBag to use it in the view

            return View(detallesRutina);
        }



        // GET: DetallesRutinas/Create
        public ActionResult Create(int? idRutina)
        {
            if (idRutina == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.idEjercicio = new SelectList(db.Ejercicios, "idEjercicio", "nombreEjercicio");
            ViewBag.idRutina = new SelectList(db.Rutina.Where(r => r.idRutina == idRutina), "idRutina", "nombreRutina", idRutina); // Pass the SelectList to the view
            ViewBag.RutinaId = idRutina; // Store idRutina in ViewBag to use it in the ActionLink

            return View();
        }

        // POST: DetallesRutinas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDetalle,idEjercicio,numRepeticiones,numSeries")] DetallesRutina detallesRutina, int idRutina)
        {
            if (ModelState.IsValid)
            {
                detallesRutina.idRutina = idRutina; // Assign the idRutina to the entity
                db.DetallesRutina.Add(detallesRutina);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = idRutina });
            }

            ViewBag.idEjercicio = new SelectList(db.Ejercicios, "idEjercicio", "nombreEjercicio", detallesRutina.idEjercicio);
            ViewBag.idRutina = new SelectList(db.Rutina, "idRutina", "nombreRutina", idRutina); // Pass the SelectList again if model state is invalid
            ViewBag.RutinaId = idRutina; // Pass idRutina to the view if model state is invalid

            return View(detallesRutina);
        }

        // GET: DetallesRutinas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DetallesRutina detallesRutina = db.DetallesRutina.Include(d => d.Ejercicios).Include(d => d.Rutina).FirstOrDefault(d => d.idDetalle == id);
            if (detallesRutina == null)
            {
                return HttpNotFound();
            }

            ViewBag.idEjercicio = new SelectList(db.Ejercicios, "idEjercicio", "nombreEjercicio", detallesRutina.idEjercicio);
            ViewBag.idRutina = new SelectList(db.Rutina.Where(r => r.idRutina == detallesRutina.idRutina), "idRutina", "nombreRutina", detallesRutina.idRutina);
            ViewBag.RutinaId = detallesRutina.idRutina; // Store idRutina in ViewBag to use it in the view

            return View(detallesRutina);
        }

        // POST: DetallesRutinas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDetalle,idEjercicio,numRepeticiones,numSeries,idRutina")] DetallesRutina detallesRutina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallesRutina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = detallesRutina.idRutina }); // Pass idRutina as parameter
            }

            ViewBag.idEjercicio = new SelectList(db.Ejercicios, "idEjercicio", "nombreEjercicio", detallesRutina.idEjercicio);
            ViewBag.idRutina = new SelectList(db.Rutina, "idRutina", "nombreRutina", detallesRutina.idRutina);
            ViewBag.RutinaId = detallesRutina.idRutina; // Pass idRutina to the view if model state is invalid

            return View(detallesRutina);
        }




        // GET: DetallesRutinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallesRutina detallesRutina = db.DetallesRutina.Find(id);
            if (detallesRutina == null)
            {
                return HttpNotFound();
            }
            ViewBag.idRutina = detallesRutina.idRutina; // Pass the idRutina to the view
            return View(detallesRutina);
        }

        // POST: DetallesRutinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetallesRutina detallesRutina = db.DetallesRutina.Find(id);
            int idRutina = detallesRutina.idRutina; // Store the idRutina before deleting
            db.DetallesRutina.Remove(detallesRutina);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = idRutina });
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
//ORIGINALLLL