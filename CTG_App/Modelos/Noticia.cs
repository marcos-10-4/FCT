using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTG_App.Modelos
{
    public class Noticia
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public DateTime Fecha { get; set; }
        public string Imagen { get; set; }
        public bool Destacada { get; set; }
    }
}
