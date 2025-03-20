using System;
using API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
  public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
  public DbSet<Resume> Resumes { get; set; }
  public DbSet<JobDescription> JobDescriptions { get; set; }
  public DbSet<MatchingResult> MatchingResults { get; set; }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder); // Required for Identity

        builder.Entity<MatchingResult>()
            .HasOne(m => m.Resume)
            .WithMany()
            .HasForeignKey(m => m.ResumeId);

        builder.Entity<MatchingResult>()
            .HasOne(m => m.JobDescription)
            .WithMany()
            .HasForeignKey(m => m.JobDescriptionId);
  }
}
