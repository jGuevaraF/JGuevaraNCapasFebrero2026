using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Municipio
    {
        public static ML.Result GetByIdEstado (int idEstado)
        {
            ML.Result result = new ML.Result ();

            try
            {

                using(DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities context = new DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities())
                {
                    var query = context.MunicipioGetByIdEstado(idEstado).ToList();

                    if(query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach(var item in query)
                        {
                            ML.Municipio municipio = new ML.Municipio();
                            municipio.IdMunicipio = item.IdMunicipio;
                            municipio.Nombre = item.Nombre;

                            result.Objects.Add(municipio);
                        }

                        result.Correct = true;
                    } else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay municipios";
                    }
                }

            } catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
