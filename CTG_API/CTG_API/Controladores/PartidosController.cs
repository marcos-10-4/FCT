using Microsoft.AspNetCore.Mvc;
using CTG_API.Data;
using CTG_API.Modelos;
using CTG_API.DTO;

namespace CTG_API.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidosController : ControllerBase
    {
        private readonly AppDb _context;

        public PartidosController(AppDb context)
        {
            _context = context;
        }

        //POST: api/partidos
        [HttpPost]
        public IActionResult RegistrarPartido(RegistrarPartidoDTO dto)
        {
            var jugador1 = _context.Usuarios.Find(dto.Jugador1Id);
            var jugador2 = _context.Usuarios.Find(dto.Jugador2Id);

            if (jugador1 == null || jugador2 == null)
                return NotFound("Jugador no encontrado");

            var partido = new Partidos
            {
                Jugador1Id = dto.Jugador1Id,
                Jugador2Id = dto.Jugador2Id,
                GanadorId = dto.GanadorId,
                Fecha = DateTime.Now
            };

            _context.Partidos.Add(partido);

            // actualizar estadísticas
            jugador1.PartidosJugados++;
            jugador2.PartidosJugados++;

            if (dto.GanadorId == jugador1.Id)
            {
                jugador1.Victorias++;
                jugador1.Puntos += 10;

                jugador2.Derrotas++;
            }
            else
            {
                jugador2.Victorias++;
                jugador2.Puntos += 10;

                jugador1.Derrotas++;
            }

            _context.SaveChanges();

            return Ok("Partido registrado");
        }

        //GET: api/partidos
        [HttpGet]
        public IActionResult GetPartidos()
        {
            var partidos = _context.Partidos
                .Select(p => new PartidoDTO
                {
                    Id = p.Id,
                    Jugador1 = _context.Usuarios
                        .Where(u => u.Id == p.Jugador1Id)
                        .Select(u => u.Nombre)
                        .FirstOrDefault(),

                    Jugador2 = _context.Usuarios
                        .Where(u => u.Id == p.Jugador2Id)
                        .Select(u => u.Nombre)
                        .FirstOrDefault(),

                    Ganador = _context.Usuarios
                        .Where(u => u.Id == p.GanadorId)
                        .Select(u => u.Nombre)
                        .FirstOrDefault(),

                    Fecha = p.Fecha
                })
                .ToList();

            return Ok(partidos);
        }
    }
}
