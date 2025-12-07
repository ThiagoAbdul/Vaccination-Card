using AuthService.Data;
using AuthService.DTOs.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    // Configurações de segurança da senha
    builder.Configuration.GetSection("IdentityOptions").Bind(options);
})
    .AddRoles<IdentityRole>() // <-- HABILITA ROLES (Funções)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Service", Version = "v1" });

    // Define o esquema de segurança (Security Scheme)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo 'Value' no formato: Bearer [seu_token]"
    });

    // Define o Requisito de Segurança (Security Requirement)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddCors(options =>
{

    var allowedOrigins = builder.Configuration
    .GetSection("CorsConfiguration:AllowedOrigins")
    .Get<string[]>() ?? [];

    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Carrega as configurações JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // A chave que será usada para VALIDAR a assinatura do token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = securityKey,

        ValidateIssuer = jwtSettings.GetValue<bool>("ValidateIssuer"),
        ValidIssuer = jwtSettings["Issuer"],

        ValidateAudience = jwtSettings.GetValue<bool>("ValidateAudience"),
        ValidAudience = jwtSettings["Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };


});

// A autorização padrão é necessária
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Invite", policy =>
    {
        policy.RequireClaim("invite");
    });
});

var app = builder.Build();

// Configuração do pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(); // Aplica o CORS

// O Identity usa estes middlewares:
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- 4. SEEDING DE ROLES E USUÁRIOS (Opcional, para testes) ---
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await SeedRolesAsync(userManager, roleManager);
}

app.Run();


// Método para criar roles e um usuário inicial
async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Admin", "User" };

    // Cria Roles
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}


// Models/TokenResponse.cs
public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
}

// Models/RegisterModel.cs
public class RegisterUserRequest : LoginRequest // Herda Email e Password
{
    // Apenas para exemplo, o Identity pode ter mais campos.
    // public string NomeCompleto { get; set; } = string.Empty;
}

// Classe de Request para o body do endpoint
public class GenerateInviteRequest
{
    public string Email { get; set; }
}