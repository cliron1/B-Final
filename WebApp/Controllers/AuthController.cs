using BezeqFinalProject.Common.Models.Auth;
using BezeqFinalProject.Common.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BezeqFinalProject.WebApp.Controllers;

[Route("auth")]
public class AuthController : Controller {
    private readonly IAuthRepo auth;
    private readonly ILogger<AuthController> logger;

    public AuthController(IAuthRepo auth, ILogger<AuthController> logger) {
        this.auth = auth;
        this.logger = logger;
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequestModel model) {
        if(!ModelState.IsValid) return View(model);

        model.Email = model.Email.ToLower().Trim();

        var user = await auth.Login(model);
        if(user == null) {
            ModelState.AddModelError("", "Failed login attempt");
            return View(model);
        }

        await signin(user, model.RemeberMe);

        return Redirect("/");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await auth.Logout(User.Claims);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }

    [HttpGet("signup")]
    [AllowAnonymous]
    public IActionResult Signup() => View();

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Signup(SignupRequestModel model) {
        if(!ModelState.IsValid) return View(model);

        model.Email = model.Email.ToLower().Trim();

        Common.Data.Entities.User user = null;
        try {
            user = await auth.Signup(model);
        } catch(Exception ex) {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }

        await signin(user, false);

        return Redirect("/");
    }

    private async Task signin(Common.Data.Entities.User user, bool remeberMe) {
        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
        };
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties {
            ExpiresUtc = !remeberMe
                ? DateTime.UtcNow.AddMinutes(20)
                : DateTime.UtcNow.AddMonths(1),
            IsPersistent = remeberMe
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}
