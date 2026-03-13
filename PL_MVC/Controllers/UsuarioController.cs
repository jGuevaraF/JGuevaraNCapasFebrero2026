using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

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
        public ActionResult Form(int? idUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();


            if (idUsuario == null)
            {
                //agregar 
                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                usuario.Direccion.Colonia.Municipio.Municipios = new List<object>();
                usuario.Direccion.Colonia.Colonias = new List<object>();
            }
            else
            {
                //editar
                ML.Result result = BL.Usuario.GetById(idUsuario.Value);

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;
                }
                else
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }

                if (usuario.Direccion.Colonia.Municipio.Estado.IdEstado > 0)
                {
                    ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);

                    if (resultMunicipio.Correct)
                    {
                        usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;
                    }
                    else
                    {
                        usuario.Direccion.Colonia.Municipio.Municipios = new List<object>();
                    }
                }

                if (usuario.Direccion.Colonia.Municipio.IdMunicipio > 0)
                {
                    ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);

                    if (resultColonia.Correct)
                    {
                        usuario.Direccion.Colonia.Colonias = resultColonia.Objects;
                    }
                    else
                    {
                        usuario.Direccion.Colonia.Colonias = new List<object>();
                    }
                }


            }

            ML.Result resultRol = BL.Rol.GetAll();

            ML.Result resultEstado = BL.Estado.GetAll();

            if (resultEstado.Correct)
            {
                usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;
            }
            else
            {
                usuario.Direccion.Colonia.Municipio.Estado.Estados = new List<object>();

            }

            usuario.Rol.Roles = resultRol.Objects;

            return View(usuario);

        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario, HttpPostedFileBase UsuarioImagen)
        {
            
            if (usuario.IdUsuario == 0)
            {
                //add

                //ML.Result result = BL.Usuario.Add(usuario);  
                ML.Result result = new ML.Result();
                result.Correct = true;

                if (result.Correct)
                {
                    return RedirectToAction("GetAll");
                    //BL.Usuario.GetAll();
                    //return View("GetAll", result);
                }
            }
            else
            {
                //update
                //BL.Usuario.Update(usuario);
            }


            return View();
        }

        [HttpGet]
        public ActionResult Delete(int idUsuario)
        {
            //ML.Result result = BL.Usuario.Delete(idUsuario);

            return RedirectToAction("GetAll", "Usuario");
        }

        [HttpGet]
        public JsonResult MunicipioGetByIdEstado(int idEstado)
        {
            ML.Result result = BL.Municipio.GetByIdEstado(idEstado);

            //conviertiendo Lista objetos => JSON

            //Serializar informacion

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ColoniaGetByIdMunicipio(int idMunicipio)
        {
            ML.Result result = BL.Colonia.GetByIdMunicipio(idMunicipio);

            return Json(result, JsonRequestBehavior.AllowGet); //CORS
        }


        [HttpGet]
        public JsonResult GetCodigoPostalByIdColonia(int idColonia)
        {
            ML.Result result = BL.Colonia.GetCodigoPostalByIdColonia(idColonia);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}