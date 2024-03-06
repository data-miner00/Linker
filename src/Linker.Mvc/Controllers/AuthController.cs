namespace Linker.Mvc.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Linker.Core.ApiModels;
using Linker.Core.Repositories;
using AutoMapper;
using Linker.Core.Models;

public sealed class AuthController : Controller
{
    private readonly IUserRepository repository;
    private readonly IMapper mapper;

    public AuthController(IUserRepository repository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(mapper);
        this.repository = repository;
        this.mapper = mapper;
    }

    // GET: AuthController/Login
    public IActionResult Login()
    {
        if (this.HttpContext.User is not null &&
            this.HttpContext.User.Identity is not null &&
            this.HttpContext.User.Identity.IsAuthenticated)
        {
            return this.RedirectToAction("Index", "Home");
        }

        return this.View();
    }

    // POST: AuthController/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var user = await this.repository
                .GetByUsernameAndPasswordAsync(request.Username, request.Password, default)
                .ConfigureAwait(false);

            Claim[] claims = [
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(ClaimTypes.DateOfBirth, user.DateOfBirth.ToShortDateString()),
            ];
            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(claimsIdentity);

            AuthenticationProperties properties = new()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            await this.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);

            return this.RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "Username or password wrong.";

            return this.View(request);
        }
        catch
        {
            this.TempData["error"] = "Something wrong";

            return this.View(request);
        }
    }

    // GET: AuthController/Register
    public IActionResult Register()
    {
        if (this.HttpContext.User is not null &&
            this.HttpContext.User.Identity is not null &&
            this.HttpContext.User.Identity.IsAuthenticated)
        {
            return this.RedirectToAction("Index", "Home");
        }

        return this.View();
    }

    // POST: AuthController/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CreateUserRequest request)
    {
        var user = this.mapper.Map<User>(request);

        try
        {
            await this.repository
                .AddAsync(user, default)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully registered.";

            return this.RedirectToAction(nameof(this.Login));
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "User already exist.";
            return this.View(request);
        }
    }

    // GET: AuthController/Logout
    public async Task<IActionResult> Logout()
    {
        await this.HttpContext
            .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return this.RedirectToAction(nameof(this.Login));
    }
}
