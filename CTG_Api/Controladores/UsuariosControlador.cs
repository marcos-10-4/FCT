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
    }
}
