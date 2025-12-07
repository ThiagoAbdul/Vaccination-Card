using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Entities;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    // O token real (string longa e aleatória)
    public string Token { get; set; } = string.Empty;

    // Data de expiração (idealmente 7 a 30 dias após a emissão)
    public DateTime Expires { get; set; }

    // Indica se foi revogado pelo usuário/sistema
    public bool IsRevoked { get; set; }

    // Chave estrangeira para o IdentityUser
    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = default!;
}