using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
  }
}
