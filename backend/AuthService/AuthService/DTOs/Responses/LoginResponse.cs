namespace AuthService.DTOs.Responses;

public class LoginResponse
{

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; }

}
