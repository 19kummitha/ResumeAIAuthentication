using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class Resume
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string UserId { get; set; }  // Identity User ID

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; }

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; }
}
