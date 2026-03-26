namespace CTG_Api.DTOs
{
    public class PerfilJugador
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Email { get; set; }
        public int Puntos { get; set; }

        public int PartidosJugados { get; set; }

        public int Victorias { get; set; }

        public int Derrotas { get; set; }
    }
}
