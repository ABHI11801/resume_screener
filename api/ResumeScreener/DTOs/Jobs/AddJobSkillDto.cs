using System.ComponentModel.DataAnnotations;

namespace ResumeScreener.DTOs.Jobs;

public class AddJobSkillDto
{
    [Required]
    public int JobId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SkillName { get; set; } =
        string.Empty;

    public bool IsRequired { get; set; } = true;

    public int Weight { get; set; } = 5;
}