using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiForm.Repository;
using WebApiForm.Repository.Models;
using WebApiForm.Services.Modelos_Tokens;

namespace WebApiForm.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly FormEncuestaDbContext _context;

        public LoginController(IConfiguration config, FormEncuestaDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost]
        [Route("User")]
        public async Task<IActionResult> InicioSesion([FromBody] Login_Model login)
        {
            string user = login.LoginUsuario;
            string pass = login.LoginPasswords;

            RegistroUsuario modelRegUsuario = await _context.RegistroUsuarios.FirstOrDefaultAsync(x => x.Usuario == user && x.Passwords == pass);

            if (modelRegUsuario == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Credenciales incorrectas",
                    result = ""
                });
            }

            var jwtConfig = _config.GetSection("Jwt").Get<Model_Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtConfig.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("id", modelRegUsuario.IdUsuarios),
                new Claim("usuario", modelRegUsuario.Usuario),
                new Claim("cedula", modelRegUsuario.Cedula),
                new Claim("email", modelRegUsuario.Email),
                new Claim("fechaCreacion", modelRegUsuario.FechaCreacion),
                new Claim("rol", modelRegUsuario.Rol),
                //new Claim("foto", modelRegUsuario.Foto),
                //new Claim("estado", modelRegUsuario.Estado),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                claims: claims,
                //expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signIn
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                success = true,
                message = "!!!Inicio de sesion exitoso!!!",
                result = tokenString,
            });
        }
    }
}
