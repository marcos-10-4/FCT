using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTG_App.Modelos
{
    public class Mensaje
    {
        public int Id { get; set; }
        public int EmisorId { get; set; }
        public int ReceptorId { get; set; }
        public string Texto { get; set; }
        public string Hora => Fecha.ToString("HH:mm");
        public DateTime Fecha { get; set; }
        [NotMapped]
        public bool EsMio { get; set; }
    }
}
