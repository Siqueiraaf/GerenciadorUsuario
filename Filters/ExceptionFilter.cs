using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GerenciadorUsuario.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private const string MensagemDeErro = "Um erro ocorreu, por favor, tente novamente!";
        public void OnException(ExceptionContext context)
        {
            var erro = new 
            {
                Mensagem = MensagemDeErro
            };

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new JsonResult(erro);
        }
    }
}