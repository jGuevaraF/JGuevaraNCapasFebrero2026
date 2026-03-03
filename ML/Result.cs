using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Result
    {
        public bool Correct { get; set; } //true o false
        public string ErrorMessage { get; set; } //Mensaje de error que va a ver el usuario

        public Exception Ex { get; set; } //Un error no esperado, no se esta contemplando - PROGRAMADOR

        public List<object> Objects { get; set; } //GetAll

        public object Object { get; set; } //GetById
    }
}
