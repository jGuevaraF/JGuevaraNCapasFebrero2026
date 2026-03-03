using System;
using System.Collections.Generic;

namespace PL
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ML.Result result = BL.Usuario.GetAll();

            if (result.Correct)
            {
                DateTime hoy = DateTime.Now;

                string soloFecha = hoy.ToString("dd-MM-yyyy");

                Console.WriteLine(soloFecha);


                foreach (ML.Usuario usuario in result.Objects)
                {
                    Console.WriteLine("Nombre Usuario " + usuario.Nombre);
                    Console.WriteLine("Nombre Rol: " + usuario.Rol.Nombre);
                }
            }

        }
    }
}
