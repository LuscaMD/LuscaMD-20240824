using GestaoColaboradores.Context;
using GestaoColaboradores.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestaoColaboradores.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly GestaoColaboradoresContext _context;

        public AutenticacaoController(IConfiguration configuration, GestaoColaboradoresContext context)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Autenticacao model)
        {
            if (IsValidUser(model.Login, model.Senha))
            {
                var token = GenerateToken(model.Login);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private bool IsValidUser(string username, string password)
        {
            if (username == "admin" && password == "password")
            {
                return true;
            }
            else
            {
                var usuario = _context.Usuario.Where(x => x.NomeLogin == username && x.Senha == password).FirstOrDefault();

                if(usuario != null)
                {
                    return true;
                }
            }

            return false;
        }

        private string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
