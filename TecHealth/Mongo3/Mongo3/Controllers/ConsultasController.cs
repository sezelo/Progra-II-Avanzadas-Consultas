using Mongo3.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using Mongo3.App_Start;

namespace Mongo3.Controllers
{
    public class ConsultasController : Controller
    {
        private IMongoCollection<PacienteModel> PacientesCollection;
        private IMongoCollection<CitasModel> CitasCollection;
        private MongoClient server;
        private MongoDBContext dbcontext;

        public ConsultasController()
        {
            dbcontext = new MongoDBContext();
            CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            PacientesCollection = dbcontext.database.GetCollection<PacienteModel>("Pacientes");
        }

        // GET: Consultas
        public ActionResult Index()
        {
            ViewBag.CitasPorPaciente = CitasPorPaciente("115530534");
            ViewBag.CitasPorEspecialidad = CitasPorEspecialidad("General");
            ViewBag.CitasPorEstado = CitasPorEstado("Cancelado");
            ViewBag.CitasPorFecha = CitasPorFecha("2018-05-25","2018-10-29");
            ViewBag.PacientesConMasCitas = PacientesConMasCitas();
            return View();
        }

        // GET: Consultas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Consultas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consultas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Consultas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Consultas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Consultas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Consultas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public int CitasPorPaciente(string cedula)
        {

            var CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            var query =
                 (from e in CitasCollection.AsQueryable<CitasModel>()
                  where e.Cedula == cedula
                  select e)
                 .Count();
            return query;
        }

        public int CitasPorFecha(string date1, string date2)
        {
            var CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            var query =
                (from e in CitasCollection.AsQueryable<CitasModel>()
                 where (e.Fecha) >= Convert.ToDateTime(date1) && (e.Fecha) <= Convert.ToDateTime(date2)
                 select e)
                 .Count();
            return query;
        }

        public int CitasPorEspecialidad(string especialidad)
        {
            var CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            var query =
                (from e in CitasCollection.AsQueryable<CitasModel>()
                 where e.Especialidad == especialidad 
                 select e)
                 .Count();
            return query;
        }

        public int CitasPorEstado(string estado)
        {
            var CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            var query =
                (from e in CitasCollection.AsQueryable<CitasModel>()
                 where e.Estado == estado 
                 select e)
                 .Count();
            return query;
        }

        public string PacientesConMasCitas()//revisar
        {
            var CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            var query =
                 (from e in CitasCollection.AsQueryable<CitasModel>()
                  orderby e.Cedula
                  select e.Cedula);
                  
            return Convert.ToString(query);
        }
    }
}