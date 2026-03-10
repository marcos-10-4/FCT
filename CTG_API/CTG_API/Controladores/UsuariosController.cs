using CTG_API.Data;
using CTG_API.DTO;
using CTG_API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace CTG_API.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDb _context;

        public UsuariosController(AppDb context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public ActionResult<List<Usuarios>> GetUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        // POST: api/usuarios/registro
        [HttpPost("registro")]
        public ActionResult<Usuarios> Registrar(Usuarios usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok(usuario);
        }
        //POST: api/usuarios/login
        [HttpPost("login")]
        public IActionResult Login(LoginDTO login)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == login.Email && u.PasswordHash == login.Password);

            if (usuario == null)
            {
                return Unauthorized("Email o contraseña incorrectos");
            }

            return Ok(usuario);
        }

        //GET: api/usuarios/ranking
        [HttpGet("ranking")]
        public IActionResult GetRanking()
        {
            var usuarios = _context.Usuarios
                .OrderByDescending(u => u.Puntos)
                .ToList();

            var ranking = usuarios
                .Select((u, index) => new RankingDTO
                {
                    Posicion = index + 1,
                    Nombre = u.Nombre,
                    Puntos = u.Puntos,
                    PartidosJugados = u.PartidosJugados,
                    Victorias = u.Victorias,
                    Derrotas = u.Derrotas
                })
                .ToList();

            return Ok(ranking);
        }

        [HttpGet("{id}")]
        public IActionResult GetPerfilJugador(int id)
        {
            var jugador = _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new PerfilJugadorDTO
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Puntos = u.Puntos,
                    PartidosJugados = u.PartidosJugados,
                    Victorias = u.Victorias,
                    Derrotas = u.Derrotas
                })
                .FirstOrDefault();

            if (jugador == null)
            {
                return NotFound("Jugador no encontrado");
            }

            return Ok(jugador);
        }
    }
}
