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
    public class EjerciciosController : Controller
    {
        private GimnasioBDEntities1 db = new GimnasioBDEntities1();

        // GET: Ejercicios
        public ActionResult Index()
        {
            var ejercicios = db.Ejercicios.Include(e => e.Maquina).Include(e => e.GrupoMuscular);
            return View(ejercicios.ToList());
        }

        // GET: Ejercicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicios ejercicios = db.Ejercicios.Find(id);
            if (ejercicios == null)
            {
                return HttpNotFound();
            }
            return View(ejercicios);
        }

        // GET: Ejercicios/Create
        public ActionResult Create()
        {
            ViewBag.idMaquina = new SelectList(db.Maquina, "idMaquina", "nombreMaquina");
            ViewBag.nombreGrupoMuscular = new SelectList(db.GrupoMuscular, "nombreGrupoMuscular", "nombreGrupoMuscular");
            return View();
        }

        // POST: Ejercicios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEjercicio,nombreEjercicio,nombreGrupoMuscular,idMaquina")] Ejercicios ejercicios)
        {
            if (ModelState.IsValid)
            {
                db.Ejercicios.Add(ejercicios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idMaquina = new SelectList(db.Maquina, "idMaquina", "nombreMaquina", ejercicios.idMaquina);
            ViewBag.nombreGrupoMuscular = new SelectList(db.GrupoMuscular, "nombreGrupoMuscular", "nombreGrupoMuscular", ejercicios.nombreGrupoMuscular);
            return View(ejercicios);
        }

        // GET: Ejercicios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicios ejercicios = db.Ejercicios.Find(id);
            if (ejercicios == null)
            {
                return HttpNotFound();
            }
            ViewBag.idMaquina = new SelectList(db.Maquina, "idMaquina", "nombreMaquina", ejercicios.idMaquina);
            ViewBag.nombreGrupoMuscular = new SelectList(db.GrupoMuscular, "nombreGrupoMuscular", "nombreGrupoMuscular", ejercicios.nombreGrupoMuscular);
            return View(ejercicios);
        }

        // POST: Ejercicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEjercicio,nombreEjercicio,nombreGrupoMuscular,idMaquina")] Ejercicios ejercicios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ejercicios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idMaquina = new SelectList(db.Maquina, "idMaquina", "nombreMaquina", ejercicios.idMaquina);
            ViewBag.nombreGrupoMuscular = new SelectList(db.GrupoMuscular, "nombreGrupoMuscular", "nombreGrupoMuscular", ejercicios.nombreGrupoMuscular);
            return View(ejercicios);
        }

        // GET: Ejercicios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicios ejercicios = db.Ejercicios.Find(id);
            if (ejercicios == null)
            {
                return HttpNotFound();
            }
            return View(ejercicios);
        }

        // POST: Ejercicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ejercicios ejercicios = db.Ejercicios.Find(id);
            db.Ejercicios.Remove(ejercicios);
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
    }
}
