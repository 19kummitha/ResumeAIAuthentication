using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class MatchingResult
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ResumeId { get; set; }

    [Required]
    public Guid JobDescriptionId { get; set; }

    [Required]
    public decimal MatchPercentage { get; set; }

    public DateTime MatchedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("ResumeId")]
    public virtual required Resume Resume { get; set; }

    [ForeignKey("JobDescriptionId")]
    public virtual required JobDescription JobDescription { get; set; }
}
