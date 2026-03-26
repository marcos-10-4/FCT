using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTG_App.Modelos
{
    internal class Mensaje
    {
        public int Id { get; set; }
        public string Emisor { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
    }
}
