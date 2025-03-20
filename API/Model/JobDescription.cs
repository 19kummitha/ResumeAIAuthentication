using System;
using System.ComponentModel.DataAnnotations;

namespace API.Model;

public class JobDescription
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(255)]
    public required string Title { get; set; }

    public required string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
