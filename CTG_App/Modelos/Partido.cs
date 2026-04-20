using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTG_App.Modelos
{
    public class Partido
    {
        public int Id { get; set; }

        public int Jugador1Id { get; set; }
        public Usuario Jugador1 { get; set; }

        public int Jugador2Id { get; set; }
        public Usuario Jugador2 { get; set; }

        public int GanadorId { get; set; }

        public DateTime Fecha { get; set; }
        
    }
}
