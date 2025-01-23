using GerenciadorUsuario.DTOs;
using GerenciadorUsuario.Models;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorUsuario.Controllers
{
    [Route("/api/usuarios")]
    [Produces("application/json")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly static List<Usuario> _usuarios = new List<Usuario>()
        {
            new Usuario()
            {
                Id = Guid.NewGuid(),
                Nome = "Usuario 01",
                Email = "usuario01@gmail.com"
            }
        };

        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        public IActionResult BuscarUsuarios() 
        {
            return Ok(_usuarios);
        }

        [HttpGet("{id:guid}", Name = nameof(BuscarPorId))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Usuario))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult BuscarPorId([FromRoute] Guid id)
        {
            Usuario usuario = _usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario is not null)
            {
                return Ok(usuario);
            }

            return NotFound();
            
        }

        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created, Type = typeof(Usuario))]
        public IActionResult CriarUsuario([FromBody] CadastrarUsuarioDTO dto)
        {
            Usuario usuario = dto.ConverterParaModelo();
            _usuarios.Add(usuario);
            return CreatedAtAction(nameof(BuscarPorId), new { usuario.Id }, usuario);
        }
    }
}