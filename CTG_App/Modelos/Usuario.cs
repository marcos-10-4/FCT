using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTG_App.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }
        public int Puntos { get; set; }
        public int PartidosJugados { get; set; }
        public int Victorias { get; set; }
        public int Derrotas { get; set; }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
