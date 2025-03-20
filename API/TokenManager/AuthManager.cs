using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.TokenManager;

public class AuthManager(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : IAuthManager
{
  private readonly UserManager<ApplicationUser> _userManager = userManager;
  private readonly RoleManager<IdentityRole> _roleManager = roleManager;
  private readonly IConfiguration _configuration = configuration;

  public async Task<IActionResult> Login(Login model)
  {
    // Implement login logic
    // Ensure to handle user authentication and token generation here
    var user = await _userManager.FindByNameAsync(model.Username);
    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
    {
      var userRoles = await _userManager.GetRolesAsync(user);
      var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
      foreach (var userRole in userRoles)
      {
        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
      }
      var token = GetToken(authClaims);
      return new OkObjectResult(new
      {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        expiration = token.ValidTo
      });
    }
    return new UnauthorizedResult();
  }

  private JwtSecurityToken GetToken(List<Claim> authClaims)
  {
    // Token generation logic
    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTAuth:Secret"]));
    var token = new JwtSecurityToken(
        expires: DateTime.Now.AddHours(3),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
    );
    return token;
  }

  public async Task<IActionResult> Register(Register model)
  {
    // Implement user registration logic
    // Handle user creation, role assignment, and response
    var userExists = await _userManager.FindByNameAsync(model.Username);
    if (userExists != null)
      return new StatusCodeResult(StatusCodes.Status500InternalServerError);

    IdentityUser user = new()
    {
      Email = model.Email,
      SecurityStamp = Guid.NewGuid().ToString(),
      UserName = model.Username
    };

    var result = await _userManager.CreateAsync((ApplicationUser)user, model.Password);
    if (!result.Succeeded)
      return new StatusCodeResult(StatusCodes.Status500InternalServerError);

    return new OkObjectResult(new Response { Status = "Success", Message = "User created successfully!" });
  }

  public async Task<IActionResult> RegisterAdmin(Register model)
  {
    // Implement admin registration logic
    // Handle user creation, role assignment (admin and user), and response
    var userExists = await _userManager.FindByNameAsync(model.Username);
    if (userExists != null)
      return new StatusCodeResult(StatusCodes.Status500InternalServerError);

    IdentityUser user = new()
    {
      Email = model.Email,
      SecurityStamp = Guid.NewGuid().ToString(),
      UserName = model.Username
    };

    var result = await _userManager.CreateAsync((ApplicationUser)user, model.Password);
    if (!result.Succeeded)
      return new StatusCodeResult(StatusCodes.Status500InternalServerError);

    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
      await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

    if (!await _roleManager.RoleExistsAsync(UserRoles.User))
      await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

    if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
      await _userManager.AddToRoleAsync((ApplicationUser)user, UserRoles.Admin);

    if (await _roleManager.RoleExistsAsync(UserRoles.User))
      await _userManager.AddToRoleAsync((ApplicationUser)user, UserRoles.User);

    return new OkObjectResult(new Response { Status = "Success", Message = "Admin user created successfully!" });
  }
}
