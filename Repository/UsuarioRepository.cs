using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorUsuario.Models;

namespace GerenciadorUsuario.Repository
{
    public class UsuarioRepository : IUsuarioRepository
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

        public List<Usuario> ObterUsuarios()
        {
            return _usuarios;
        }

        // Simulando uma chamada assíncrona
        public async Task<List<Usuario>> ObterUsuariosAsync()
        {
            await Task.Delay(4_000);
            return _usuarios;
        }
    }
}