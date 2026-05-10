using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeScreener.Models;

[Table("Jobs")]
public class Job
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CreatedByUserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "Open";

    public decimal MinimumScore { get; set; } = 60;

    public DateTime CreatedAt { get; set; } =
        DateTime.UtcNow;
}