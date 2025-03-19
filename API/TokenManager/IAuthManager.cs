using System;
using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.TokenManager;

public interface IAuthManager
{
  Task<IActionResult> Login(Login model);
  Task<IActionResult> Register(Register model);
  Task<IActionResult> RegisterAdmin(Register model);
}
