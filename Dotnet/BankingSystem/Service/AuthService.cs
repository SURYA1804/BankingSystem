using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTO;
using interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using MyDbContext;

namespace Service;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly MyAppDbContext context;

    public AuthService(IConfiguration config, MyAppDbContext context)
    {
        _config = config;
        this.context = context;
    }

    public async  Task<string> GenerateJwtToken(UserDTO user)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // var role = await context.DbRoles.FirstAsync(r=>r.RoleId == user.RoleId);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Name,user.UserName ),
            new Claim(ClaimTypes.Role, user.RoleName!) 
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}