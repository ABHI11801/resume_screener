using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeScreener.Models;

[Table("Scores")]
public class Score
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ResumeId { get; set; }

    [Required]
    public int JobId { get; set; }

    public decimal TotalScore { get; set; }

    public string MatchedSkills { get; set; } =
        string.Empty;

    public string MissingSkills { get; set; } =
        string.Empty;

    public string Explanation { get; set; } =
        string.Empty;

    public DateTime CreatedAt { get; set; } =
        DateTime.UtcNow;
}