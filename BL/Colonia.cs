using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Colonia
    {
        public static ML.Result GetByIdMunicipio(int idMunicipio)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities context = new DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities())
                {
                    var query = context.ColoniaGetByIdMunicipio(idMunicipio).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Colonia colonia = new ML.Colonia();
                            colonia.IdColonia = item.IdColonia;
                            colonia.Nombre = item.Nombre;

                            result.Objects.Add(colonia);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay colonias";
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

        public static ML.Result GetCodigoPostalByIdColonia(int idColonia)
        {
            ML.Result result = new ML.Result();
            try
            {

                using (DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities context = new DL_EF.JGuevaraProgramacioNCapasFebrero2026Entities())
                {
                    //var query = context.Colonias.Select(model => model.CodigoPostal).Where();

                    var query = (from colonia in context.Colonias
                                 where colonia.IdColonia == idColonia
                                 select new
                                 {
                                     CodigoPostal = colonia.CodigoPostal
                                 }).SingleOrDefault();

                    if (query != null)
                    {
                        result.Object = query.CodigoPostal;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "NO se encontro la colonia o el codigo postal";
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
    }
}
