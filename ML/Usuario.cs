using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }

        public string FechaNacimiento { get; set; }

        public byte[] Imagen { get; set; }

        public bool Estatus { get; set; }
        //propiedades de navegacion

        public ML.Rol Rol { get; set; } //FK

        public ML.Direccion Direccion { get; set; }


        public List<object> Usuarios { get; set; } //guardar usuarios de la BD
    }
}
