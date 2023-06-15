using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Services.Description;
using PanelControlSolar.Datos;
using PanelControlSolar.Models;

namespace PanelControlSolar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Index()
        {
            if (Session["usuarioLogeado"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Registro()
        {
            return View();
        }

        public ActionResult Administrar()
        {
            if (Session["usuarioLogeado"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult CerrarSesion()
        {
            Session.Remove("usuarioLogeado");
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public JsonResult DatosTiempo()
        {
            DT_DatosTiempo datos = new DT_DatosTiempo();

            //Por defecto mostraremos los últimos 20 días en el periodo de fechas
            DateTime fechaminima = DateTime.Now.AddDays(-20);
            DateTime fechamaxima = DateTime.Now;

            List<ListaDatosTiempo> objLista = datos.RetornarDatosTiempo(fechaminima,fechamaxima);

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult APIDatosTiempo(string[] fechas)
        {
            API_DatosTiempo api = new API_DatosTiempo();

            //DateTime fechaminima = DateTime.Now.AddDays(-20);
            //DateTime fechamaxima = DateTime.Now;

            string [] temperaturas = api.ObtenerTemperaturasRangoFechas(fechas);
            return Json(temperaturas, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Acceder (string usuario, string pass)
        {
            Usuario user = new Usuario();
            
            if (user.ComprobarUsuario(usuario,pass))
            {
                Session["usuarioLogeado"] = usuario;

                CodigoPostal cp = new CodigoPostal();
                cp.ObtenerCodigoPostal();

                API_DatosTiempo aPI_DatosTiempo = new API_DatosTiempo();
                aPI_DatosTiempo.DatosAPI();

                return RedirectToAction("Index", "Home"); // Redireccionar temperaturas página de inicio
            
            } else
            {
                TempData["Error"] = "Usuario o contraseña incorrectos";
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult Registrarse(string usuario, string pass)
        {
            Usuario user = new Usuario();

            if (user.RegistrarUsuario(usuario, pass))
            {
                TempData["SuccessMessage"] = "Usuario registrado. Acceda con sus datos.";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                TempData["Error"] = "No se ha podido crear el usuario";
                return RedirectToAction("Registro", "Home");
            }
        }

        [HttpPost]
        public ActionResult ProcesarFechas(DateTime fechaMinima, DateTime fechaMaxima)
        {
            DT_DatosTiempo datos = new DT_DatosTiempo();

            List<ListaDatosTiempo> objLista = datos.RetornarDatosTiempo(fechaMinima,fechaMaxima);

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ModificarCodigoPostal (string codigopostal)
        {
            CodigoPostal cp = new CodigoPostal();
            cp.ModificarCodigo(codigopostal);

            TempData["SuccessMessage"] = "Codigo postal modificado.";
            return RedirectToAction("Administrar", "Home");
        }

        [HttpPost]
        public ActionResult EliminarUsuario ()
        {
            Usuario user = new Usuario();
            
            if (user.EliminarUsuario(Usuario.Nombre))
                return RedirectToAction("Login", "Home");

            else return View();
        }
    }
}