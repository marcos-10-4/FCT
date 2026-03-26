using CTG_Api.Data;
using CTG_Api.DTOs;
using CTG_Api.Modelos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CTG_Api.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosControlador : ControllerBase
    {
        private readonly AppDb _context;

        public UsuariosControlador(AppDb context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public ActionResult<List<Usuario>> GetUsuarios()
        {
            return _context.Usuarios.ToList();
        }
        [HttpPost("login")]
        public IActionResult Login(InicioSesion login)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == login.Email && u.PasswordHash == login.Password);

            if (usuario == null)
            {
                return Unauthorized("Email o contraseña incorrectos");
            }

            return Ok(usuario);
        }
        // POST: api/usuarios/registro
        [HttpPost("registro")]
        public ActionResult<Usuario> Registrar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok(usuario);
        }
        [HttpGet("ranking")]
        public ActionResult<List<Ranking>> GetRanking()
        {
            var usuarios = _context.Usuarios
                                   .OrderByDescending(u => u.Puntos)
                                   .ToList();

            var ranking = usuarios.Select((u, index) => new Ranking
            {
                Posicion = index + 1,
                Nombre = u.Nombre,
                Puntos = u.Puntos
            }).ToList();

            return Ok(ranking);
        }
        [HttpGet("{id}")]
        public IActionResult GetPerfilJugador(int id)
        {
            var jugador = _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new PerfilJugador
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Puntos = u.Puntos,
                    PartidosJugados = u.PartidosJugados,
                    Victorias = u.Victorias,
                    Derrotas = u.Derrotas
                })
                .FirstOrDefault();

            if (jugador == null)
                return NotFound("Jugador no encontrado");

            return Ok(jugador);
        }
    }
}
