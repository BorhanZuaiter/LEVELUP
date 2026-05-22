using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace API.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, token);

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured")));

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claims = jwtToken.Claims;
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity(claims, "jwt");
            context.User = new System.Security.Claims.ClaimsPrincipal(claimsIdentity);
        }
        catch
        {
            // Token validation failed, continue without user context
        }
    }
}
