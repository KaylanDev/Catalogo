using Catalogo.DTOs.Registres;
using Catalogo.Models;
using Catalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService,
        UserManager<AplicationUsers> userManeger,
        RoleManager<IdentityRole> roleManeger, IConfiguration config,
        ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManeger = userManeger;
        _roleManeger = roleManeger;
        _config = config;
        _logger = logger;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManeger.FindByNameAsync(model.UserName!);

        if (user is not null || await _userManeger.CheckPasswordAsync(user!, model.Passwaord))
        {
            var userRoles = await _userManeger.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim("id",user.UserName!),
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

        var result = await _userManeger.CreateAsync(user, model.Passwaord!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = "500",
                Menssage = "falha na criação!"
            });
        }

        return Ok(new Response { Status = "success", Menssage = "User created sucessfully" });
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
            user.RefreshTokenExpiryTime >= DateTime.UtcNow)
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


    [HttpPost]
    [Route("revoke/{username}")]
    [Authorize(Policy = "ExclusivePolicyOnly")]
    public async Task<ActionResult> Revoke(string username)
    {
        var user = await _userManeger.FindByNameAsync(username);
        if (user is null) return BadRequest("username is invalid");


        user.RefreshToken = null;

        await _userManeger.UpdateAsync(user);

        return NoContent();
    }

    [HttpPost]
    [Route("Create-role")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<ActionResult> CreateRole(string role)
    {
        var roleExist = await _roleManeger.RoleExistsAsync(role);

        if (!roleExist)
        {
            var roleResult = _roleManeger.CreateAsync(new IdentityRole(role));
            if (roleResult.Result.Succeeded)
            {
                _logger.LogInformation(1, "Role Add");
                return StatusCode(StatusCodes.Status201Created, new Response
                {
                    Status = "sucess",
                    Menssage = $"role {role} add successfuly! "
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    Status = "Error",
                    Menssage = $"issue adding the new {role} role"
                });
            }
        }
        return StatusCode(StatusCodes.Status400BadRequest, new Response
        {
            Status = "Error",
            Menssage = $"role {role} exist"
        });
    }

    [HttpPost]
    [Route("AddUserToRole")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<ActionResult> AddUserToRole(string email, string role)
    {
        var userExist = await _userManeger.FindByEmailAsync(email);

        if (userExist != null)
        {
            var result = await _userManeger.AddToRoleAsync(userExist, role);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "Add User to role succeded!");
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "success",
                    Menssage = "user add to role susscced"
                });
            }
            else
            {
                _logger.LogInformation(1, $"Error: Unable to user add to {role}");
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    Status = "Error",
                    Menssage = $"Unable add {email} to the {role} role"
                });

            }
        }
        return BadRequest(new
        {
            error = "Unable to find user"
        });

    }
}
