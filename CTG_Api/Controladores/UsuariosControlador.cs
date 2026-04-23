using CTG_Api.Data;
using CTG_Api.DTOs;
using CTG_Api.Modelos;
using CTG_Api.Servicios;
using CTG_Api.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CTG_Api.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosControlador : ControllerBase
    {
        private readonly AppDb _context;
        private readonly JwtServicios _jwtService;
        public UsuariosControlador(AppDb context,JwtServicios jwtServicio)
        {
            _context = context;
            _jwtService = jwtServicio;
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
                .FirstOrDefault(u => u.Email == login.Email);

            if (usuario == null)
            {
                return Unauthorized("Usuario no encontrado");
            }

            if (!SeguridadContraseña.VerificarPassword(login.Password, usuario.PasswordHash))
            {
                return Unauthorized("Contraseña incorrecta");
            }

            var token = _jwtService.GenerarToken(usuario);

            return Ok(new
            {
                token,
                usuario
            });
        }
        // POST: api/usuarios/registro
        [HttpPost("registro")]
        public ActionResult<Usuario> Registrar(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.PasswordHash) || usuario.PasswordHash.Length < 6)
            {
                return BadRequest("La contraseña debe tener al menos 6 caracteres");
            }
            if (string.IsNullOrEmpty(usuario.Rol))
            {
                usuario.Rol = "Usuario";
            }
            usuario.PasswordHash = SeguridadContraseña.HashPassword(usuario.PasswordHash);

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
            {
                return NotFound("Jugador no encontrado");
            }

            return Ok(jugador);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }
            var partidos = _context.Partidos
        .Where(p => p.Jugador1Id == id || p.Jugador2Id == id);

            _context.Partidos.RemoveRange(partidos);
            _context.Usuarios.Remove(usuario);

            _context.SaveChanges();

            return Ok("Usuario eliminado correctamente");
        }
    }
}
