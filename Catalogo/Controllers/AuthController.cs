using Catalogo.DTOs.Registres;
using Catalogo.Models;
using Catalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catalogo.Controllers;

public class AuthController : Controller
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<AplicationUsers> _userManeger;
    private readonly RoleManager<IdentityRole> _roleManeger;
    private readonly IConfiguration _config;

    public AuthController(ITokenService tokenService,
        UserManager<AplicationUsers> userManeger,
        RoleManager<IdentityRole> roleManeger, IConfiguration config)
    {
        _tokenService = tokenService;
        _userManeger = userManeger;
        _roleManeger = roleManeger;
        _config = config;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManeger.FindByNameAsync(model.UserName!);

        if (user is not null || await _userManeger.CheckPasswordAsync(user, model.Passwaord))
        {
            var userRoles = await _userManeger.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAcessToken(authClaims, _config);

            var refreshToken = _tokenService.RegenerateRefreshToken();

            _ = int.TryParse(_config["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

            await _userManeger.UpdateAsync(user);
            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromBody] RegistreModel model)
    {
        var userExist = await _userManeger.FindByNameAsync(model.UserName);

        if (userExist is not null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = "500",
                Menssage = "usuario ja existe!"
            });
        }

        AplicationUsers user = new()
        {
            UserName = model.UserName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManeger.CreateAsync(user,model.Passwaord!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = "500",
                Menssage = "falha na criação!"
            });
        }

        return Ok(new Response { Status = "success",Menssage = "User created sucessfully"});
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<ActionResult> RefreshToken([FromBody] TokenModel model)
    {
        if (model is null)
        {
            return BadRequest("Token is null");
        }

        string? acessToken = model.AcessToken ?? throw new ArgumentNullException(nameof(model));
        string? refreshToken = model.RefreshToken ?? throw new ArgumentNullException(nameof(model));

        var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken, _config);

        if (principal is null)
        {
            return BadRequest("invalid acess token/refresh token");
        }

        string username = principal.Identity.Name;

        var user = await _userManeger.FindByNameAsync(username!);

        if (user is null || user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("invalid acess token/refresh token");
        }

        var newAcessToken = _tokenService.GenerateAcessToken(principal.Claims.ToList(), _config);
        var newRefreshToken = _tokenService.RegenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManeger.UpdateAsync(user);

        return new ObjectResult(new
        {
           acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
           refreshToken = newRefreshToken
        });
    }
    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<ActionResult> Revoke(string username)
    {
        var user = await _userManeger.FindByNameAsync(username);
        if (user is null) return BadRequest("username is invalid");


        user.RefreshToken = null;

        await _userManeger.UpdateAsync(user);

        return NoContent();
    }




}
