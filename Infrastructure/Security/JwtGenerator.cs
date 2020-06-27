using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security
{
  public class JwtGenerator : IJWTGenerator
  {
    private readonly SymmetricSecurityKey _key;
    public JwtGenerator(IConfiguration config)
    {
      _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }

    public string CreateToken(AppUser user)
    {
      var claims=new List<Claim>{
        //This is going to add user name to token which we are going to send back.
        new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
      };
      //generate signin credentials before it leaves the api 
      var creds= new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor=new SecurityTokenDescriptor{
        Subject=new ClaimsIdentity(claims),
        Expires=DateTime.Now.AddDays(7),
        SigningCredentials=creds
      };

      var tokenHandler=new JwtSecurityTokenHandler();
      var token=tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
      
    }
  }
}