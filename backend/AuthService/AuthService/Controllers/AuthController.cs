using AuthService.Data;
using AuthService.DTOs.Requests;
using AuthService.DTOs.Responses;
using AuthService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    UserManager<IdentityUser> userManager,
    ApplicationDbContext dbContext,
    IConfiguration configuration) : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly ApplicationDbContext _dbContext = dbContext; // Injete o DBContext
    private readonly IConfiguration _configuration = configuration;

    // ==========================================================
    // ENDPOINT 1: CADASTRO (/api/auth/register)
    // ==========================================================

    [HttpPost("register-root-user")]
    public async Task<IActionResult> RegisterRootUser([FromBody] RegisterUserRequest model)
    {

        var hasUsers = await _userManager.Users.AnyAsync();

        if (hasUsers)
        {
            return BadRequest(new { Message = "Usuário root já cadastrado" });
        }

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };

        var transaction = await _dbContext.Database.BeginTransactionAsync();

        // Cria o usuário com o Identity

        try
        {
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Message = "Falha no cadastro", Errors = errors });
            }

            await userManager.AddToRoleAsync(user, "Admin"); // Adiciona a Role "Admin"

            await transaction.CommitAsync();

            return StatusCode(201, new { Message = "Usuário registrado com sucesso. Por favor, faça login." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    [HttpPost("register")]
    [Authorize(Policy = "Invite")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest model)
    {
        var subjectId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(subjectId == null || subjectId != model.Email)
            return Unauthorized();

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };

        // Cria o usuário com o Identity
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Message = "Falha no cadastro", Errors = errors });
        }


        return StatusCode(201, new { Message = "Usuário registrado com sucesso. Por favor, faça login." });
    }


    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(200)]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized(new { Message = "Credenciais inválidas." });
        }

        // 1. Invalida Refresh Tokens antigos (se desejar)
        var existingTokens = await _dbContext.RefreshTokens
            .Where(t => t.UserId == user.Id && !t.IsRevoked)
            .ToListAsync();
        existingTokens.ForEach(t => t.IsRevoked = true);

        // 2. Cria e Salva o novo Refresh Token
        var refreshToken = CreateRefreshToken(user.Id);
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        // 3. Cria o Access Token (JWT)
        var accessToken = await GenerateJwtToken(user);
        var expiresIn = (int)accessToken.ValidTo.Subtract(DateTime.UtcNow).TotalSeconds;

        // 4. Resposta de Login
        return Ok(new LoginResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            TokenType = "Bearer",
            ExpiresIn = expiresIn,
            RefreshToken = refreshToken.Token // Retorna o Refresh Token
        });
    }


    [HttpPost("refresh-token")]
    [ProducesResponseType<LoginResponse>(200)]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var storedToken = await _dbContext.RefreshTokens
            .Include(t => t.User) // Inclui o IdentityUser
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        // 1. Verifica se o Refresh Token é válido
        if (storedToken == null || storedToken.IsRevoked || storedToken.Expires < DateTime.UtcNow)
        {
            return Unauthorized(new { Message = "Refresh Token inválido, revogado ou expirado." });
        }

        var user = storedToken.User;
        if (user == null)
        {
            return Unauthorized(new { Message = "Usuário não encontrado." });
        }

        // 2. Revoga o Refresh Token atual (Regra de Reuso Único)
        storedToken.IsRevoked = true;

        // 3. Gera um NOVO Access Token e um NOVO Refresh Token
        var newAccessToken = await GenerateJwtToken(user);
        var newRefreshToken = CreateRefreshToken(user.Id);

        await _dbContext.RefreshTokens.AddAsync(newRefreshToken);
        await _dbContext.SaveChangesAsync();

        var expiresIn = (int)newAccessToken.ValidTo.Subtract(DateTime.UtcNow).TotalSeconds;

        // 4. Retorna os novos tokens
        return Ok(new LoginResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            TokenType = "Bearer",
            ExpiresIn = expiresIn,
            RefreshToken = newRefreshToken.Token // Retorna o novo Refresh Token
        });
    }

    // ==========================================================
    // MÉTODOS PRIVADOS
    // ==========================================================

    // Gera o JWT usando a chave simétrica (MESMO CÓDIGO DO ANTERIOR)
    private async Task<JwtSecurityToken> GenerateJwtToken(IdentityUser user)
    {
        // ... (Implementação do GenerateJwtToken do exemplo anterior) ...
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
        var tokenExpiryMinutes = double.Parse(jwtSettings["ExpiryMinutes"]!);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        );

        return new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenExpiryMinutes),
            signingCredentials: signingCredentials
        );
    }

    // Cria a nova entidade RefreshToken
    private RefreshToken CreateRefreshToken(string userId)
    {
        // Cria uma string de token longa e criptograficamente segura
        var secureRandomToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        int expirationsHours = _configuration.GetSection("JwtSettings").GetValue<int>("RefreshTokenExpirationInHours");

        return new RefreshToken
        {
            Token = secureRandomToken,
            Expires = DateTime.UtcNow.AddHours(expirationsHours), 
            IsRevoked = false,
            UserId = userId
        };
    }

    [HttpPost("generate-invite-token")]
    public IActionResult GenerateInviteToken([FromBody] GenerateInviteRequest request)
    {
        // 1. Defina a chave secreta e o emissor
        var secretKey = _configuration["JwtSettings:Key"];
        var issuer = _configuration["JwtSettings:Issuer"];

        if (string.IsNullOrEmpty(secretKey))
        {
            return StatusCode(500, "JWT Secret Key não configurada.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 2. Crie as Claims
        var claims = new List<Claim>
        {
            // Claim padrão para identificar o usuário (sub = Subject)
            new(JwtRegisteredClaimNames.Sub, request.Email),
            // Claim padrão para o ID do Token (opcional, mas bom para rastreio)
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            
            // *************** Claim Customizada: 'invite' ***************
            // Esta claim sinaliza que o token é para fins de convite/cadastro
            new("invite", "true")
        };

        // 3. Crie o Token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer, // Geralmente a audiência é o próprio emissor ou o nome da API
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), // Define o tempo de vida do token (30 minutos)
            signingCredentials: creds
        );

        // 4. Retorne o token gerado em formato string
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
