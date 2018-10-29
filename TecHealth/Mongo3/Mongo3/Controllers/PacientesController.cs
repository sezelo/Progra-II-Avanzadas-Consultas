using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using Mongo3.App_Start;
using Mongo3.Models;
using MongoDB.Driver.Linq;

namespace Mongo3.Controllers
{
    public class PacientesController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<PacienteModel> pacienteCollection;
        private IMongoCollection<CitasModel> CitasCollection;
        //var collection = database.GetCollection<TDocument>("collectionname");
        public PacientesController()
        {
            dbcontext = new MongoDBContext();
            pacienteCollection = dbcontext.database.GetCollection<PacienteModel>("Pacientes");
        }

        // GET: Funcionarios
        public ActionResult Index()
        {
            List<PacienteModel> pacientes = pacienteCollection.AsQueryable<PacienteModel>().ToList();
            return View(pacientes);
        }

        // GET: Funcionarios/Details/5
        public ActionResult Details(string id)
        {
            var pacientesId = new ObjectId(id);
            var pacientes = pacienteCollection.AsQueryable<PacienteModel>().SingleOrDefault(x => x.Id == pacientesId);
            return View(pacientes);
        }

        // GET: Funcionarios/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Funcionarios/Create
        [HttpPost]
        public ActionResult Create(PacienteModel pacientes)
        {
            try
            {
                pacienteCollection.InsertOne(pacientes);

                return RedirectToAction("Index"); 
            }
            catch
            {
                return View();
            }
        }

        // GET: Funcionarios/Edit/5
        public ActionResult Edit(string id)
        {
            var pacienteId = new ObjectId(id);
            var paciente = pacienteCollection.AsQueryable<PacienteModel>().SingleOrDefault(x => x.Id == pacienteId);
            return View(paciente);
           
        }

        // POST: Funcionarios/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, PacienteModel pacientes)
        {
            try
            {
                var filter = Builders<PacienteModel>.Filter.Eq("id_", ObjectId.Parse(id));
                var update = Builders<PacienteModel>.Update.Set("Nombre", pacientes.Nombre);//Se puede agregar mas haciendo un .Set("",) extra
                var result = pacienteCollection.UpdateOne(filter, update);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Funcionarios/Delete/5
        public ActionResult Delete(string id)
        {
            var pacientesId = new ObjectId(id);
            var pacientes = pacienteCollection.AsQueryable<PacienteModel>().SingleOrDefault(x => x.Id == pacientesId);
            return View(pacientes);
        }

        // POST: Funcionarios/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, PacienteModel pacientes)
        {
            try
            {
                pacienteCollection.DeleteOne(Builders<PacienteModel>.Filter.Eq("_id", ObjectId.Parse(id)));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(PacienteModel user)
        {
                var usr = pacienteCollection.AsQueryable<PacienteModel>().SingleOrDefault(u => u.Nombre == user.Nombre && u.Contraseña == user.Contraseña);
                //var usr = funcionarioCollection.FindAsync(u => u.Nombre == user.Nombre && u.Contraseña == user.Contraseña);
                if (usr != null)
                {

                    return RedirectToAction("Index", "Citas");
                }
                else
                {
                    ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos");
                }
                return View();
            
        }

        public object CitasPorPaciente()
        {
            var query =
                 (from e in pacienteCollection.AsQueryable<PacienteModel>()
                 //where e.Cedula == "115530534" //Aqui va el valor del textbox
                 select e)
                 .Count(e => e.Cedula == "115530534");
            return query;
            //Console.WriteLine("Esta es el query: "+query+"/n");
        }

        public object CitasPorFecha()
        {
            DateTime date1 = Convert.ToDateTime("31/12/2018"); //Aqui irian los textbox que contienen fechas
            DateTime date2 = Convert.ToDateTime("31/12/2019");
            var query =
                (from e in CitasCollection.AsQueryable<CitasModel>()
                 where DateTime.Parse(e.Fecha) >= date1 && DateTime.Parse(e.Fecha) <= date2
                 select e)
                 .Count();
            return query;
            //Console.WriteLine(query);
        }

        public void CitasPorEspecialidad()
        {
            var query =
                (from e in CitasCollection.AsQueryable<CitasModel>()
                 where e.Especialidad == "General" //aqui iria el textbox que contiene la especialidad
                 select e)
                 .Count();
            Console.WriteLine(query);
        }

        public void CitasPorEstado()
        {
            var query =
                (from e in CitasCollection.AsQueryable<CitasModel>()
                 where e.Estado == "Cancelado" //aqui iria el textbox que contiene la estado
                 select e)
                 .Count();
            Console.WriteLine(query);
        }

        public void PacientesConMasCitas()
        {
            var query =
                 from e in pacienteCollection.AsQueryable<PacienteModel>()
                 orderby (e.Cedula.Count())
                 //where e. == "115530534" //Aqui va el valor del textbox
                 select (e.Nombre);
            Console.WriteLine(query);
        }

    }

    
}
