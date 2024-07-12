using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pruebaFGRP.Models;
using pruebaFGRP.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuth.WebApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly pruebaFGRPContext _context;

        public TokenController(IConfiguration config, pruebaFGRPContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("DisplayName", user.DisplayName),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);
                    string message = "Usuario: "+ user.UserName + " accedió con éxito.";// Creamos un mensaje para registrar la acción.
                    Log Log = new Log
                    {
                        Message = message,
                        Level = 0,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Logs.Add(Log);
                    await _context.SaveChangesAsync();

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    string message = "Usuario: " + _userData.Email + " ingresó mal las contraseñas.";// Creamos un mensaje para registrar la acción.
                    Log Log = new Log
                    {
                        Message = message,
                        Level = 0,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<UserInfo> GetUser(string email, string password)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}