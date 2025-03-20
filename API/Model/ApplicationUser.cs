using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Model;

public class ApplicationUser:IdentityUser
{
    [Required]
    [MaxLength(100)]
    public required string FullName { get; set; }
}
