using CTG_Api.Data;
using CTG_Api.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTG_Api.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatControlador : ControllerBase
    {
        private readonly AppDb _context;

        public ChatControlador(AppDb context)
        {
            _context = context;
        }

        // Obtener mensajes
        [HttpGet]
        public ActionResult<List<Mensaje>> GetMensajes()
        {
            return _context.Mensajes
                           .OrderBy(m => m.Fecha)
                           .ToList();
        }

        // Enviar mensaje
        [HttpPost]
        public ActionResult EnviarMensaje(Mensaje mensaje)
        {
            _context.Mensajes.Add(mensaje);
            _context.SaveChanges();

            return Ok();
        }
        //Chat entre dos usuarios
        [HttpGet("{usuario1}/{usuario2}")]
        public ActionResult<IEnumerable<Mensaje>> GetChat(int usuario1, int usuario2)
        {
            var mensajes = _context.Mensajes
                .Where(m =>
                    (m.EmisorId == usuario1 && m.ReceptorId == usuario2) ||
                    (m.EmisorId == usuario2 && m.ReceptorId == usuario1))
                .OrderBy(m => m.Fecha)
                .ToList();

            return mensajes;
        }
    }
}
