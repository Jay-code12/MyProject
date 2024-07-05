using Api.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services
{
    public class JwtService
    {
        private readonly IConfiguration config;
        private readonly SymmetricSecurityKey jwtKey;
        public JwtService(IConfiguration config)
        {
            this.config = config;
            jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["JWT:Key"]));
        }

        public string createJwt(User user)
        {
                
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FIrstName),
                new Claim(ClaimTypes.Surname, user.LastName)

            };

            var credentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(config["JWT:ExpiresDay"])),
                SigningCredentials = credentials,
                Issuer = config["JWT:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescripter);
            
            return tokenHandler.WriteToken(jwt);

        }
    }
}
