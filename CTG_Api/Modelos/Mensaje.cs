using System.ComponentModel.DataAnnotations.Schema;

namespace CTG_Api.Modelos
{
    public class Mensaje
    {
        public int Id { get; set; }
        public int EmisorId { get; set; }
        public int ReceptorId { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        // Propiedades para UI (no mapeadas a la BD)
        [NotMapped]
        public string Hora => Fecha.ToString("HH:mm");

        [NotMapped]
        public bool EsMio { get; set; }
    }
}
