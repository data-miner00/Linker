namespace Linker.Mvc.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Repositories;
using AutoMapper;
using Linker.Core.V2.Models;
using ILinkerAuthenticationHandler = Linker.Data.SqlServer.IAuthenticationHandler;

public sealed class AuthController : Controller
{
    private readonly IUserRepository repository;
    private readonly IMapper mapper;
    private readonly ILinkerAuthenticationHandler authenticationHandler;

    public AuthController(IUserRepository repository, IMapper mapper, ILinkerAuthenticationHandler authenticationHandler)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(mapper);
        this.repository = repository;
        this.mapper = mapper;
        this.authenticationHandler = authenticationHandler;
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

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
    public async Task<IActionResult> Login(LoginRequest request, string returnUrl)
    {
        try
        {
            var user = await this.repository
                .GetByUsernameAsync(request.Username, default)
                .ConfigureAwait(false);

            var isAuthenticated = await this.authenticationHandler
                .LoginAsync(user.Id, request.Password, default)
                .ConfigureAwait(false);

            if (!isAuthenticated)
            {
                throw new InvalidOperationException();
            }

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

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return this.LocalRedirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException)
        {
            this.TempData[Constants.Error] = "Username or password wrong.";

            return this.View(request);
        }
        catch
        {
            this.TempData[Constants.Error] = "Something wrong";

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
            if (this.ModelState.IsValid)
            {
                await this.repository
                    .AddAsync(user, this.CancellationToken)
                    .ConfigureAwait(false);

                await this.authenticationHandler
                    .RegisterAsync(user.Id, request.Password, default)
                    .ConfigureAwait(false);

                this.TempData[Constants.Success] = "Successfully registered.";

                return this.RedirectToAction(nameof(this.Login));
            }

            return this.View(request);
        }
        catch (InvalidOperationException)
        {
            this.TempData[Constants.Error] = "User already exist.";
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
