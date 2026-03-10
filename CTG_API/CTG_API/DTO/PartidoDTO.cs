namespace CTG_API.DTO
{
    public class PartidoDTO
    {
        public int Id { get; set; }

        public string Jugador1 { get; set; }
        public string Jugador2 { get; set; }

        public string Ganador { get; set; }

        public DateTime Fecha { get; set; }
    }
}
