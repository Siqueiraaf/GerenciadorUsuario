using GerenciadorUsuario.Models;

namespace GerenciadorUsuario.Repository
{
    public interface IUsuarioRepository
    {
        public List<Usuario> ObterUsuarios();
        Task<List<Usuario>> ObterUsuariosAsync();
    }
}