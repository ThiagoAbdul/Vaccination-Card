using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthService.Services;

public class AuthService(UserManager<IdentityUser> userManager) : IAuthService
{

}
