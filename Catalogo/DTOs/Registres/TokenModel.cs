using Microsoft.AspNetCore.Server.HttpSys;

namespace Catalogo.DTOs.Registres
{
    public class TokenModel
    {
        public string? AcessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
