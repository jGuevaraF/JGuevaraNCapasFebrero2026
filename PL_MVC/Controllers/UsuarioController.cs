using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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


            ML.Usuario usuario = new ML.Usuario();
            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol = new ML.Rol();
            usuario.Rol.IdRol = 0;


            ML.Result result = BL.Usuario.GetAll(usuario);


            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;

            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
            }

            ML.Result resultRol = BL.Rol.GetAll();
            usuario.Rol.Roles = resultRol.Objects;

            if (Session["UsuariosCarga"] != null)
            {
                ML.Usuario cargaMasiva = Session["UsuariosCarga"] as ML.Usuario;
                usuario.Correctos = cargaMasiva.Correctos;
                usuario.Incorrectos = cargaMasiva.Incorrectos;
            }

            return View(usuario);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuarioBuscar)
        {
            ML.Usuario usuario = new ML.Usuario();
            if (ModelState.IsValid)
            {
                //validos correctos
                //if (usuario.Nombre == null)
                //{
                //    usuario.Nombre = "";
                //}

                usuarioBuscar.Nombre = usuarioBuscar.BusquedaAbierta.Nombre == null ? "" : usuarioBuscar.Nombre;

                //if(usuario.ApellidoPaterno == null)
                //{
                //    usuario.ApellidoPaterno = ""; ??
                //}

                usuarioBuscar.ApellidoPaterno = usuario.ApellidoPaterno == null ? "" : usuario.ApellidoPaterno;

                if (usuarioBuscar.ApellidoMaterno == null)
                {
                    usuarioBuscar.ApellidoMaterno = "";
                }


                ML.Result result = BL.Usuario.GetAll(usuario);
                usuario.Usuarios = result.Objects;


            }
            else
            {
                usuario.Usuarios = new List<object>();

            }

            ML.Result resultRol = BL.Rol.GetAll();
            usuario.Rol.Roles = resultRol.Objects;

            return View(usuario);

        }

        //FileResult => descarga un archivo
        //JsonResult => regresa un JSON

        [HttpGet]
        public ActionResult Form(int? idUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Municipios = new List<object>();
            usuario.Direccion.Colonia.Colonias = new List<object>();


            if (idUsuario == null)
            {
                //agregar 

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

            if (ModelState.IsValid)
            {
                //HttpPostedFileBase imagen = Request.Files["UsuarioImagen"];

                if (UsuarioImagen != null)
                {
                    //convertir httpPostedFileBase => byte[]
                    // Source - https://stackoverflow.com/a/7852256
                    // Posted by Jon Skeet
                    // Retrieved 2026-03-13, License - CC BY-SA 3.0

                    byte[] data;
                    using (Stream inputStream = UsuarioImagen.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }

                        usuario.Imagen = memoryStream.ToArray();
                    }

                }

                if (usuario.IdUsuario == 0)
                {
                    //add

                    ML.Result result = BL.Usuario.Add(usuario);
                    //ML.Result result = new ML.Result();
                    //result.Correct = true;

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
                    BL.Usuario.UpdateEF(usuario);
                    return RedirectToAction("GetAll");
                }
            }
            else
            {
                ML.Result resultRol = BL.Rol.GetAll();
                usuario.Rol.Roles = resultRol.Objects;

                ML.Result resultEstado = BL.Estado.GetAll();
                usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;

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
                else
                {
                    usuario.Direccion.Colonia.Municipio.Municipios = new List<object>();

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
                else
                {
                    usuario.Direccion.Colonia.Colonias = new List<object>();

                }

                return View(usuario);
            }

            return View(usuario);


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

        public ActionResult CargaMasiva(HttpPostedFileBase archivoTxt)
        {
            //ext archivo .txt .xlx
            ML.Usuario usuarioVista = new ML.Usuario();
            usuarioVista.Correctos = new List<object>();
            usuarioVista.Incorrectos = new List<object>();

            if (archivoTxt != null)
            {
                using (var reader = new StreamReader(archivoTxt.InputStream))
                {
                    bool encabezados = true;
                    string linea;

                    int numeroLinea = 1;
                    //linea = reader.ReadLine();
                    while ((linea = reader.ReadLine()) != null)
                    {

                        if (encabezados)
                        {
                            encabezados = false;
                            numeroLinea++;
                            continue;
                        }

                        string[] datos = linea.Split('|');

                        string validacion = ValidarDatos(datos, numeroLinea);

                        if (validacion == "Todo bien")
                        {

                            ML.Usuario usuario = new ML.Usuario();
                            usuario.Nombre = datos[1];
                            usuario.ApellidoPaterno = datos[2];
                            usuario.ApellidoMaterno = datos[3];

                            usuarioVista.Correctos.Add(usuario);
                        }
                        else
                        {
                            usuarioVista.Incorrectos.Add($"Linea {numeroLinea}: {validacion}");
                        }
                        numeroLinea++;
                    }
                }
                Session["UsuariosCarga"] = usuarioVista;
            }
            else
            {
                if (Session["UsuariosCarga"] != null)
                {
                    ML.Usuario cargaMasiva = Session["UsuariosCarga"] as ML.Usuario;
                    if (cargaMasiva != null && cargaMasiva.Incorrectos.Count == 0)
                    {
                        foreach (ML.Usuario usuario in cargaMasiva.Correctos)
                        {
                            BL.Usuario.Add(usuario);
                        }
                    }
                }
                Session["UsuariosCarga"] = null;
            }
            return RedirectToAction("GetAll");
        }


        public string ValidarDatos(string[] datos, int numeroLinea)
        {
            try
            {
                //if (datos.Length < 6)
                //{
                //    throw new Exception("faltan datos");
                //}

                if (!Regex.IsMatch(datos[1], "[a-zA-Z]"))
                {
                    throw new Exception("Verica el nombre");
                }


                return "Todo bien";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}