using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OAuthDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ITokenStorage _tokenStorage;

    public AuthController(ITokenService tokenService, ITokenStorage tokenStorage)
    {
        _tokenService = tokenService;
        _tokenStorage = tokenStorage;
    }

    [HttpGet("signin")]
    public IActionResult SignIn()
    {
        var authorizationUrl = _tokenService.GenerateAuthorizationUrl();
        return Redirect(authorizationUrl);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback(string code, string state)
    {
        var (accessToken, refreshToken, expiresIn) = await _tokenService.ExchangeAuthorizationCodeAsync(code);
        await _tokenStorage.StoreTokensAsync(accessToken, refreshToken, expiresIn);
        return Redirect("/"); // Redirect the user to the application
    }
}