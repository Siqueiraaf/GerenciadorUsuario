using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace GerenciadorUsuario.Controllers
{
    [Route("/api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string Secret;

        public LoginController(IConfiguration configuration)
        {
            Secret = configuration.GetValue<string>("ChaveAutenticacao");
        }

        /// <summary>
        /// Endpoint Responsável por gerar o token do usuário no modelo JWT
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [EnableRateLimiting("janela-fixa")]
        public ActionResult GerarToken()
        {
            // Definição da nossa chave de acesso
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Definição das claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, "usuario@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("ler-dados-por-id", "true")
            };

            var token = new JwtSecurityToken(
                issuer: "usuarios-api",
                audience: "usuarios-api",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(tokenString);
        }
    }
}