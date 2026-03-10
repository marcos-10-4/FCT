using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion_CTG.Modelos
{
    internal class Ranking
    {
        public int Posicion { get; set; }
        public string Nombre { get; set; }
        public int Puntos { get; set; }
        public int PartidosJugados { get; set; }
        public int Victorias { get; set; }
        public int Derrotas { get; set; }
    }
}
