using CTG_Api.Data;
using CTG_Api.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace CTG_Api.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiasControlador : ControllerBase
    {
        private readonly AppDb _context;

        public NoticiasControlador(AppDb context)
        {
            _context = context;
        }

        // Obtener noticias
        [HttpGet]
        public ActionResult<List<Noticia>> GetNoticias()
        {
            return _context.Noticias
                .OrderByDescending(n => n.Fecha)
                .ToList();
        }

        // Crear noticia
        [HttpPost]
        public ActionResult CrearNoticia(Noticia noticia)
        {
            noticia.Fecha = DateTime.Now;

            _context.Noticias.Add(noticia);
            _context.SaveChanges();

            return Ok();
        }
        // Eliminar noticia
        [HttpDelete("{id}")]
        public IActionResult EliminarNoticia(int id)
        {
            var noticia = _context.Noticias.Find(id);

            if (noticia == null)
            {
                return NotFound();
            }

            _context.Noticias.Remove(noticia);
            _context.SaveChanges();

            return Ok();
        }
    }
}
