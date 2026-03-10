namespace CTG_API.Modelos
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        // Admin, Socio, Entrenador
        public string Rol { get; set; } 
        public int Puntos { get ; set ; } 
        public int PartidosJugados { get ; set ; } 
        public int Victorias { get ; set ; } 
        public int Derrotas { get ; set ; }
    }
}
