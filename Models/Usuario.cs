using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorUsuario.Models
{
    public class Usuario
    {
        public Guid Id { get; set;}
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}