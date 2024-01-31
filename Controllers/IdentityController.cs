using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notes_with_tagging.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notes_with_tagging.Controllers
{
    public class IdentityController : ControllerBase
    {
        private readonly JwtOptions _jwtOptions;
        //public IdentityController(IOptions<JwtOptions> jwtOptions) 
        //{
        //    _jwtOptions = jwtOptions.Value;
        //}
        public IdentityController(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        [HttpPost("token"), Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> GenerateToken([FromForm] object request)
        {
            HttpContext ctx = this.HttpContext;

            var tokenExpiration = TimeSpan.FromSeconds(_jwtOptions.ExpirationSeconds);
            var accessToken = CreateAccessToken(
                _jwtOptions,
                "fdsfs",
                TimeSpan.FromMinutes(60),
                new[] { "read_todo", "create_todo" });

            //returns a json response with the access token
            return Ok(new
            {
                access_token = accessToken,
                expiration = (int)tokenExpiration.TotalSeconds,
                type = "bearer"
            });


            return Ok(ctx.Request.ContentType);
        }

        private static string CreateAccessToken(
            JwtOptions jwtOptions,
            string username,
            TimeSpan expiration,
            string[] permissions)
        {
            var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(
                symmetricKey,
                // 👇 one of the most popular. 
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("sub", username),
                new Claim("name", username),
                new Claim("aud", jwtOptions.Audience)
            };

            var roleClaims = permissions.Select(x => new Claim("role", x));
            claims.AddRange(roleClaims);

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
