﻿using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Blazor.Infrastructure.Services.JWT;

public class AccessTokenGenerator : IAccessTokenGenerator
{
    private readonly SimpleJwtOptions _options;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public AccessTokenGenerator(JwtSecurityTokenHandler tokenHandler, IOptions<SimpleJwtOptions> options)
    {
        _tokenHandler = tokenHandler;
        _options = options.Value;
    }

    public string GenerateAccessToken(ClaimsPrincipal user)
    {
        var signingOptions = _options.AccessSigningOptions!;
        var credentials = new SigningCredentials(signingOptions.SigningKey, signingOptions.Algorithm);
        var header = new JwtHeader(credentials);
        var payload = new JwtPayload(
            _options.Issuer!,
            _options.Audience!,
            user.Claims,
            DateTime.Now,
            DateTime.Now.AddMinutes(signingOptions.ExpirationMinutes)
        );
        return _tokenHandler.WriteToken(new JwtSecurityToken(header, payload));
    }
}