using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Notes_with_tagging.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notes_with_tagging.Controllers
{
    public class IdentityController : ControllerBase
    {
        private readonly JwtOptions _jwtOptions;

        public IdentityController(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        [HttpPost("token"), Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> GenerateToken([FromForm] AccessTokenData request)
        {
            if (request == null || request.Surname == null || request.Name == null)
            {
                return BadRequest("Please provide your name and surname");
            }

            var tokenExpiration = TimeSpan.FromSeconds(_jwtOptions.ExpirationSeconds);
            var accessToken = CreateAccessToken(
                _jwtOptions,
                request,
                TimeSpan.FromMinutes(60));

            return Ok(new
            {
                access_token = accessToken,
                expiration = (int)tokenExpiration.TotalSeconds,
                type = "bearer",
                issued_for = request
            });
        }

        private static string CreateAccessToken(
            JwtOptions jwtOptions,
            AccessTokenData request,
            TimeSpan expiration)
        {
            var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(
                symmetricKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("surname", request.Surname),
                new Claim("name", request.Name),
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.Add(expiration),
                signingCredentials: signingCredentials);

            var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            return rawToken;
        }
    }
}
