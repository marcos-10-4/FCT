using CTG_Api.Data;
using CTG_Api.DTOs;
using CTG_Api.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CTG_Api.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidosControlador : ControllerBase
    {
        private readonly AppDb _context;

        public PartidosControlador(AppDb context)
        {
            _context = context;
        }
        [Authorize(Roles = "Entrenador")]
        [HttpPost]
        public IActionResult RegistrarPartido(RegistrarPartido dto)
        {
            var jugador1 = _context.Usuarios.Find(dto.Jugador1Id);
            var jugador2 = _context.Usuarios.Find(dto.Jugador2Id);

            if (jugador1 == null || jugador2 == null)
            {
                return NotFound("Jugador no encontrado");
            }

            var partido = new Partido
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

        [HttpPost("partido")]
        public IActionResult CrearPartido([FromBody] PartidoCrearDTO dto)
        {
            var partido = new Partido
            {
                Jugador1Id = dto.Jugador1Id,
                Jugador2Id = dto.Jugador2Id,
                GanadorId = dto.GanadorId,
                Fecha = DateTime.Now
            };

            _context.Partidos.Add(partido);

            // 👉 actualizar estadísticas
            var ganador = _context.Usuarios.Find(dto.GanadorId);
            ganador.Victorias++;
            ganador.Puntos += 3;

            var perdedorId = dto.Jugador1Id == dto.GanadorId ? dto.Jugador2Id : dto.Jugador1Id;
            var perdedor = _context.Usuarios.Find(perdedorId);
            perdedor.Derrotas++;

            ganador.PartidosJugados++;
            perdedor.PartidosJugados++;

            _context.SaveChanges();

            return Ok();
        }
    }
}
