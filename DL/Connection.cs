using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class Connection
    {
        public static string GetConnection()
        {

            //return "Data Source=.;Initial Catalog=PrimeraSemana;User ID=sa;password=pass@word1;Encrypt=False";

            return ConfigurationManager.ConnectionStrings["JGuevaraProgramacionNCapasFebrero2026"].ToString();
        }
    }
}
