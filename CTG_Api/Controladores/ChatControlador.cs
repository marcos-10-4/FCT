using CTG_Api.Data;
using CTG_Api.Modelos;
using Microsoft.AspNetCore.Mvc;

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
    }
}
