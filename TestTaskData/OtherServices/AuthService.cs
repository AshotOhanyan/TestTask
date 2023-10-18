using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskData.OtherServices
{
    public static class AuthService
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only.", nameof(password));

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only.", nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }

            return true;
        }

        public static SymmetricSecurityKey GenerateSecurityKey()
        {
            string secret = "RmFzdGx5IEp3dCBTdHJpbmcgU2VjdXJpdHkgS2V5"; // This should be a secure secret, not a simple string

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
        public static string GenerateAccessToken(string id, string name)
        {
            Random r = new Random();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,id),
                new Claim(ClaimTypes.Name,name),
                new Claim("tokenId",r.Next().ToString())
            };

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "https://localhost:7157/",
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
                signingCredentials: new SigningCredentials(AuthService.GenerateSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
