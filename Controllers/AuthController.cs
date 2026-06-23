using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancasAPI.Data;
using FinancasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario login)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == login.Email);

        if (usuario == null || usuario.Senha != login.Senha)
            return Unauthorized("Usuário ou senha inválidos");

        var token = GerarToken(usuario);

        return Ok(new
        {
            token,
            usuario = new
            {
                usuario.Id,
                usuario.Email
            }
        });
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok("Usuário criado com sucesso");
    }

    // JWT
    private string GerarToken(Usuario usuario)
    {
        var jwtSettings = _config.GetSection("Jwt");

        var keyString = jwtSettings["Key"];
        if (string.IsNullOrEmpty(keyString))
            throw new Exception("JWT Key não configurada no appsettings.json");

        var key = Encoding.UTF8.GetBytes(keyString);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}