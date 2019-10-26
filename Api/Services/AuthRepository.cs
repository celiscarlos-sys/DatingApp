using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Api.Data;
using Api.Dtos.Auth;
using Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DatingAppContext _datingAppContext;
        private readonly IConfiguration _configuration;
        public AuthRepository(DatingAppContext datingAppContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _datingAppContext = datingAppContext;
        }

        public UserToLoginResponse loginUser(UserToLogin userToLogin)
        {
            var usrFromDb = _datingAppContext.Usuarios.FirstOrDefault(x => x.Username == userToLogin.Username);
            if (usrFromDb == null) { return null; }
            if (!comparePasswords(userToLogin.Password, usrFromDb.PassHash, usrFromDb.PassSalt)) { return null; }
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, usrFromDb.Cedula),
                new Claim(ClaimTypes.Name, usrFromDb.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AddSection:myKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(1)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var myToken = tokenHandler.WriteToken(token);
            UserToLoginResponse userToLoginResponse = new UserToLoginResponse()
            {
                Token = myToken
            };
            return userToLoginResponse;
        }

        private bool comparePasswords(string password, byte[] passHash, byte[] passSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passSalt))
            {
                var computeHashDb = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHashDb.Length; i++)
                {
                    if (computeHashDb[i] != passHash[i]) { return false; }
                }
                return true;
            }
        }

        public bool registerUser(UserToRegister userToRegister)
        {
            var usrFromDb = _datingAppContext.Usuarios.FirstOrDefault(x => x.Username == userToRegister.Username);
            if (usrFromDb != null) { return false; }
            byte[] passHash, passSalt;
            generateHashSalt(userToRegister.Password, out passHash, out passSalt);
            Usuario usertoCreate = new Usuario()
            {
                Cedula = userToRegister.Cedula,
                Username = userToRegister.Username,
                PassHash = passHash,
                PassSalt = passSalt
            };
            _datingAppContext.Usuarios.Add(usertoCreate);
            _datingAppContext.SaveChanges();
            return true;
        }

        private void generateHashSalt(string password, out byte[] passHash, out byte[] passSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                passSalt = hmac.Key;
            }
        }
    }
}
