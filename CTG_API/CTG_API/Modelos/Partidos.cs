namespace CTG_API.Modelos
{
    public class Partidos
    {
        public int Id { get; set; }

        public int Jugador1Id { get; set; }
        public Usuarios Jugador1 { get; set; }

        public int Jugador2Id { get; set; }
        public Usuarios Jugador2 { get; set; }

        public int GanadorId { get; set; }

        public DateTime Fecha { get; set; }
        public bool Validado { get; set; }
    }
}
