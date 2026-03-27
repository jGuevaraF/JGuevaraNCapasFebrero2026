using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public string Nombre { get; set; }

        [Required]
        [DisplayName("Apellido Paterno")]
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }

        public string FechaNacimiento { get; set; }

        public byte[] Imagen { get; set; }

        public bool Estatus { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //propiedades de navegacion

        public ML.Rol Rol { get; set; } //FK

        public ML.Direccion Direccion { get; set; }


        public List<object> Usuarios { get; set; } //guardar usuarios de la BD


        public ML.BusquedaAbierta BusquedaAbierta { get; set; }


        //manejo de los errores
        public List<object> Correctos { get; set; }
        public List<object> Incorrectos { get; set; }
    }
}
