using System.Net.Mime;
using GerenciadorUsuario.DTOs;
using GerenciadorUsuario.Models;
using GerenciadorUsuario.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorUsuario.Controllers
{
    [Route("/api/usuarios")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        public IActionResult BuscarUsuarios([FromQuery] string filtroNome = "") 
        {
            throw new Exception("Erro não Tratado");

            IEnumerable<Usuario> usuariosFiltrados = _usuarioRepository.ObterUsuarios().Where(u => u.Nome.StartsWith(filtroNome, StringComparison.OrdinalIgnoreCase));
            return Ok(usuariosFiltrados);
        }

        [HttpGet("{id:guid}", Name = nameof(BuscarPorId))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Usuario))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult BuscarPorId([FromRoute] Guid id)
        {
            Usuario usuario = _usuarioRepository.ObterUsuarios().FirstOrDefault(u => u.Id == id);
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
            _usuarioRepository.ObterUsuarios().Add(usuario);
            return CreatedAtAction(nameof(BuscarPorId), new { usuario.Id }, usuario);
        }

        [HttpPatch("{id:guid}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public IActionResult AtualizarUsuario([FromRoute] Guid id, [FromBody] AtualizarUsuarioDTO dto)
        {
            Usuario usuario = _usuarioRepository.ObterUsuarios().FirstOrDefault(u => u.Id == id);
            if (usuario is null)
            {
                return NotFound();
            }
            usuario.Nome = dto.Nome;
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult RemoverUsuario([FromRoute] Guid id)
        {
            Usuario usuario = _usuarioRepository.ObterUsuarios().FirstOrDefault(u => u.Id == id);
            if (usuario is null)
            {
                return NotFound();
            }

            _usuarioRepository.ObterUsuarios().Remove(usuario);
            return NoContent();
        }
    }
}