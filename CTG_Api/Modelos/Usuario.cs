namespace CTG_Api.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; } // Admin, Socio, Entrenador
        public int Puntos { get; set; }
        public int PartidosJugados { get; set; }
        public int Victorias { get; set; }
        public int Derrotas { get; set; }
    }
}
