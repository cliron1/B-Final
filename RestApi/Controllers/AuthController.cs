using BezeqFinalProject.Common.Models.Auth;
using BezeqFinalProject.Common.Repos;
using BezeqFinalProject.WebApi.Filters;
using BezeqFinalProject.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BezeqFinalProject.WebApi.Controllers;

[ApiController]
[Route("auth")]
[TypeFilter(typeof(StopwatchFilter))]
public class AuthController : ControllerBase {
    private readonly IAuthRepo auth;
    private readonly JwtSettings jwtSettings;
    private readonly ILogger<AuthController> logger;

    public AuthController(IAuthRepo auth, IConfiguration config, ILogger<AuthController> logger) {
        this.auth = auth;
        jwtSettings = config.GetSection("Jwt").Get<JwtSettings>();
        this.logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginApiResponse>> Login(LoginRequestModel model) {
        model.Email = model.Email.Trim().ToLower();

        var user = await auth.Login(model);
        if(user == null)
            return BadRequest("Failed login attempt");

        var token = signin(user);

        return new LoginApiResponse { Token = token, User = user };
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginApiResponse>> Signup(SignupRequestModel model) {
        model.Email = model.Email.Trim().ToLower();

        Common.Data.Entities.User user = null;
        try {
            user = await auth.Signup(model);
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }

        var token = signin(user);

        return new LoginApiResponse { Token = token, User = user };
    }

    private string signin(Common.Data.Entities.User user) {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
        };

        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(5),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }
}
