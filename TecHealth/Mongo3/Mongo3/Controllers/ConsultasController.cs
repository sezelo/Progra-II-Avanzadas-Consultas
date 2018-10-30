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
        private IMongoCollection<TratamientoModel> TratamientosCollection;
        private IMongoCollection<DiagnosticoModel> DiagnosticoCollection;
        private MongoClient server;
        private MongoDBContext dbcontext;

        public ConsultasController()
        {
            dbcontext = new MongoDBContext();
            CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            PacientesCollection = dbcontext.database.GetCollection<PacienteModel>("Pacientes");
            TratamientosCollection = dbcontext.database.GetCollection<TratamientoModel>("Tratamientos");
            DiagnosticoCollection = dbcontext.database.GetCollection<DiagnosticoModel>("Diagnostico");
        }

        // GET: Consultas
        public ActionResult Index()
        {
            ViewBag.CitasPorPaciente = CitasPorPaciente("115530534");
            ViewBag.CitasPorEspecialidad = CitasPorEspecialidad("General");
            ViewBag.CitasPorEstado = CitasPorEstado("Cancelado");
            ViewBag.CitasPorFecha = CitasPorFecha("2018-05-25", "2018-10-29");
            ViewBag.PacientesConMasCitas = PacientesConMasCitas();
            ViewBag.CantidadTratamiento = CantidadTratamiento("ZZZ");
            ViewBag.Promedio = PromedioMonto("ZZZ");
            ViewBag.EnfermedadesMasDiagnosticadasNombre = EnfermedadesMasDiagnosticadasNombre();
            ViewBag.EnfermedadesMasDiagnosticadasCantidad = EnfermedadesMasDiagnosticadasCantidad();
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

        //Consultas de Citas
        public int CitasPorPaciente(string cedula) //devuelve la cantidad de citas por paciente
        {

            var CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            var query =
                 (from e in CitasCollection.AsQueryable<CitasModel>()
                  where e.Cedula == cedula
                  select e)
                 .Count();
            return query;
        }

        public int CitasPorFecha(string date1, string date2) //devuelve cantidad de citas por rango de fechas
        {
            var CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            var query =
                (from e in CitasCollection.AsQueryable<CitasModel>()
                 where (e.Fecha) >= Convert.ToDateTime(date1) && (e.Fecha) <= Convert.ToDateTime(date2)
                 select e)
                 .Count();
            return query;
        }

        public int CitasPorEspecialidad(string especialidad) //devuelve cantidad de citas por especialidad
        {
            var CitasCollection = dbcontext.database.GetCollection<CitasModel>("Citas");
            var query =
                (from e in CitasCollection.AsQueryable<CitasModel>()
                 where e.Especialidad == especialidad
                 select e)
                 .Count();
            return query;
        }

        public int CitasPorEstado(string estado) //devuelve cantidad de citas por estado
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
                  select e.Cedula).ToList();
            var paciente = //devuelve el paciente con mas citas
                query.GroupBy(s => s)
                .OrderByDescending(s => s.Count())
                .First().Key;
            return paciente;
        }

        //Consultas de Tratamientos
        public int CantidadTratamiento(string tratamiento) //cantidad de veces que es asignado el tratamiento
        {
            var TratamientosCollection = dbcontext.database.GetCollection<TratamientoModel>("Tratamientos");
            var query =
                (from e in TratamientosCollection.AsQueryable<TratamientoModel>()
                 where e.Nombre == tratamiento
                 select e)
                 .Count();
            return query;
        }

        public double PromedioMonto(string tratamiento) //monto promedio de los tratamientos asignados de ese tipo
        {
            var TratamientosCollection = dbcontext.database.GetCollection<TratamientoModel>("Tratamientos");
            var query =
                (from e in TratamientosCollection.AsQueryable<TratamientoModel>()
                 where e.Nombre == tratamiento
                 select e.Monto)
                 .Average();
            return query;
        }

        //Consulta de Enfermedades mas diagnosticadas
        public string EnfermedadesMasDiagnosticadasNombre()
        {
            var DiagnosticoCollection = dbcontext.database.GetCollection<DiagnosticoModel>("Diagnostico");
            var query =
                (from e in DiagnosticoCollection.AsQueryable<DiagnosticoModel>()
                 select e.Nombre).ToList();
            var nombreD = //devuelve la enfermedad mas diagnosticada
                query.GroupBy(s => s)
                .OrderByDescending(s => s.Count())
                .First().Key;
            return nombreD;
        }

        public string EnfermedadesMasDiagnosticadasCantidad()
        {
            var DiagnosticoCollection = dbcontext.database.GetCollection<DiagnosticoModel>("Diagnostico");
            var query =
                (from e in DiagnosticoCollection.AsQueryable<DiagnosticoModel>()
                 select e.Nombre).ToList();
            var cantNombreD = //devuelve la cantidad de la enfermedad mas diagnosticada
                 query.GroupBy(s => s)
                .OrderByDescending(s => s.Count())
                .Count().ToString();
            return cantNombreD;
        }
    }
}