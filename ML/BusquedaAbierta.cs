using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class BusquedaAbierta
    {
        [RegularExpression("")]
        public string Nombre { get; set; }
        [RegularExpression("")]
        public string ApellidoPaterno { get; set; }

        [RegularExpression("")]
        public string ApellidoMaterno { get; set; }

        public int IdRol { get; set; }

        public List<object> Roles { get; set; }
    }
}
