namespace CTG_Api.DTOs
{
    public class Ranking
    {
        public int Posicion { get; set; } // nueva propiedad
        public string Nombre { get; set; }
        public int Puntos { get; set; }
        public int PartidosJugados { get; set; }
        public int Victorias { get; set; }
        public int Derrotas { get; set; }
    }
}
