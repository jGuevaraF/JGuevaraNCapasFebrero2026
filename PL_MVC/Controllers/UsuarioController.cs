using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_MVC.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario

        //todas las peticiones

        [HttpGet] //Action Verb | Decoradores
        public ActionResult GetAll()
        {
            ML.Result result = BL.Usuario.GetAll();

            ML.Usuario usuario = new ML.Usuario();

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;

            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;

            }



            return View(usuario);
        }

        //FileResult => descarga un archivo
        //JsonResult => regresa un JSON

        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
    }
}