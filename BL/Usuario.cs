using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        //public static bool Add(ML.Usuario usuario)
        //{
        //    using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = context;
        //        cmd.CommandText = "UsuarioAdd"; //INSERT, UPDATE, DELETE, SELECT
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        //        cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
        //        cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
        //        cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);


        //    }
        //}

        public static ML.Result GetAllADONET()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = context;
                    cmd.CommandText = "UsuarioGetAll";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        //obtener la informacion

                        result.Objects = new List<object>();

                        foreach (DataRow row in dataTable.Rows)
                        {
                            ML.Usuario usuario = new ML.Usuario();
                            usuario.IdUsuario = Convert.ToInt32(row[0]);
                            usuario.Nombre = row[1].ToString();
                            usuario.ApellidoPaterno = row[2].ToString();
                            usuario.ApellidoMaterno = row[3].ToString();



                            usuario.Rol = new ML.Rol();

                            if (row[4] == DBNull.Value)
                            {
                                usuario.Rol.IdRol = 0;
                            }
                            else
                            {

                                usuario.Rol.IdRol = Convert.ToInt32(row[4]);
                            }

                            usuario.Rol.Nombre = row[5].ToString();

                            result.Objects.Add(usuario);

                        }

                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registros";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result GetByIdADO(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = context;
                    cmd.CommandText = "SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario";

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        //obtener la informacion

                        DataRow row = dataTable.Rows[0];

                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = Convert.ToInt32(row[0]);
                        usuario.Nombre = row[1].ToString();
                        usuario.ApellidoPaterno = row[2].ToString();
                        usuario.ApellidoMaterno = row[3].ToString();



                        usuario.Rol = new ML.Rol();

                        if (row[4] == DBNull.Value)
                        {
                            usuario.Rol.IdRol = 0;
                        }
                        else
                        {

                            usuario.Rol.IdRol = Convert.ToInt32(row[4]);
                        }

                        //usuario.Rol.Nombre = row[5].ToString();


                        result.Object = usuario;


                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registros";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }


        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities context = new DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities())
                {

                    var query = (from usuario in context.Usuarios
                                 join rol in context.Rols on usuario.IdRol equals rol.IdRol into UsuarioRol //inner join
                                 from usuarioRol in UsuarioRol.DefaultIfEmpty()

                                 join direccion in context.Direccions on usuario.IdUsuario equals direccion.IdUsuario into UsuarioDireccion
                                 from usuarioDireccion in UsuarioDireccion.DefaultIfEmpty()

                                 join colonia in context.Colonias on usuarioDireccion.IdColonia equals colonia.IdColonia into DireccionColonia
                                 from direccionColonia in DireccionColonia.DefaultIfEmpty()

                                     ///joins
                                     ///



                                 select new
                                 {
                                     IdUsuario = usuario.IdUsuario,
                                     UsuarioNombre = usuario.Nombre,
                                     ApellidoPaterno = usuario.ApellidoPaterno,
                                     ApellidoMaterno = usuario.ApellidoMaterno,
                                     RolNombre = usuarioRol.Nombre,

                                     Calle = usuarioDireccion.Calle,
                                     NumeroInterior = usuarioDireccion.NumeroInterior

                                 }).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Usuario usuario = new ML.Usuario();
                            usuario.IdUsuario = item.IdUsuario;
                            usuario.Nombre = item.UsuarioNombre;
                            usuario.ApellidoPaterno = item.ApellidoPaterno;
                            usuario.ApellidoMaterno = item.ApellidoMaterno;

                            usuario.Rol = new ML.Rol();
                            usuario.Rol.Nombre = item.RolNombre;

                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.Calle = item.Calle;
                            usuario.Direccion.NumeroInterior = item.NumeroInterior;
                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                            //con lo demas

                            result.Objects.Add(usuario);

                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registros";
                    }

                }



            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result GetById(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities context = new DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities())
                {
                    var query = context.UsuarioGetById(idUsuario).SingleOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;

                        usuario.Rol = new ML.Rol();
                        if (query.IdRol == null)
                        {
                            usuario.Rol.IdRol = 0;
                        }
                        else
                        {
                            usuario.Rol.IdRol = query.IdRol.Value;

                        }

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.Calle = query.Calle;
                        usuario.Direccion.NumeroExterior = query.NumeroExterior;
                        usuario.Direccion.NumeroInterior = query.NumeroInterior;

                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = query.IdColonia;
                        usuario.Direccion.Colonia.CodigoPostal = query.CodigoPostal;

                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = query.IdMunicipio;

                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = query.IdEstado;

                        result.Object = usuario;

                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existe el usuario";
                    }
                }


            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result AddLINQ(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities context = new DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities())
                {
                    //1. Crear una instancia de la tabla
                    DL_EF.Usuario usuarioAdd = new DL_EF.Usuario();

                    //2. Pasar la informacion de ML.Usuario => DL_EF.Usuario
                    usuarioAdd.Nombre = usuario.Nombre;
                    usuarioAdd.ApellidoPaterno = usuario.ApellidoPaterno;
                    usuarioAdd.ApellidoMaterno = usuario.ApellidoMaterno;

                    //3. Pasarle la informacion a DL

                    context.Usuarios.Add(usuarioAdd);

                    //4.Guardar cambios => ejecuta el query
                    int filasAfectadas = context.SaveChanges();

                    int ultimoId = usuarioAdd.IdUsuario; //ultimo id

                    if (filasAfectadas > 0)
                    {

                        //investigar como obtener el ultimo id de usuario insertado
                        DL_EF.Direccion direccionAdd = new DL_EF.Direccion();
                        direccionAdd.IdUsuario = 1084975;
                        direccionAdd.Calle = usuario.Direccion.Calle;

                        direccionAdd.NumeroExterior = usuario.Direccion.NumeroExterior;

                        context.Direccions.Add(direccionAdd);

                        int filasAfectasDireccion = context.SaveChanges();

                        if (filasAfectasDireccion > 0)
                        {
                            result.Correct = true;

                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "Error al insertar direccion";
                        }


                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Hay un error";
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }


        public static ML.Result DeleteLINQ(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities context = new DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities())
                {
                    //Borrar direccion

                    //1 buscarlo

                    var query = (from usuario in context.Usuarios
                                 where usuario.IdUsuario == idUsuario
                                 select usuario).SingleOrDefault();

                    if (query != null)
                    {
                        //2 eliminarlo
                        context.Usuarios.Remove(query);

                        int filasAfectadas = context.SaveChanges();

                        if (filasAfectadas > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo eliminar";
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }


        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities context = new DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities())
                {
                    var query = context.UsuarioAdd(usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno, usuario.Imagen, usuario.Rol.IdRol, usuario.Direccion.Calle, usuario.Direccion.NumeroInterior, usuario.Direccion.NumeroExterior, usuario.Direccion.Colonia.IdColonia);


                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
