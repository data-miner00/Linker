namespace Linker.Mvc.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Repositories;
using AutoMapper;
using Linker.Core.V2.Models;
using Linker.Common.Helpers;
using Serilog;
using ILinkerAuthenticationHandler = Linker.Data.SqlServer.IAuthenticationHandler;

public sealed class AuthController : Controller
{
    private readonly IUserRepository repository;
    private readonly IMapper mapper;
    private readonly ILinkerAuthenticationHandler authenticationHandler;
    private readonly ILogger logger;

    public AuthController(
        IUserRepository repository,
        IMapper mapper,
        ILinkerAuthenticationHandler authenticationHandler,
        ILogger logger)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.mapper = Guard.ThrowIfNull(mapper);
        this.authenticationHandler = Guard.ThrowIfNull(authenticationHandler);
        this.logger = Guard.ThrowIfNull(logger);
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
        if (!this.ModelState.IsValid)
        {
            return this.View(request);
        }

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
        catch (InvalidOperationException ex)
        {
            this.TempData[Constants.Error] = "Username or password wrong.";
            this.logger.Warning(ex, "Invalid login for {username}.", request.Username);

            return this.View(request);
        }
        catch (Exception ex)
        {
            this.TempData[Constants.Error] = "Something wrong";
            this.logger.Error("Login for {username} failed due to {exception}", request.Username, ex.Message);

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
            if (!this.ModelState.IsValid)
            {
                return this.View(request);
            }

            await this.repository
                .AddAsync(user, this.CancellationToken)
                .ConfigureAwait(false);

            await this.authenticationHandler
                .RegisterAsync(user.Id, request.Password, default)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Successfully registered.";

            return this.RedirectToAction(nameof(this.Login));
        }
        catch (InvalidOperationException ex)
        {
            this.TempData[Constants.Error] = "User already exist.";
            this.logger.Error(ex, "Unable to register. {message}", ex.Message);
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
