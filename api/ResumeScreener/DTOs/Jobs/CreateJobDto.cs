using System.ComponentModel.DataAnnotations;

namespace ResumeScreener.DTOs.Jobs;

public class CreateJobDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Department { get; set; } =
        string.Empty;

    public string Description { get; set; } =
        string.Empty;

    public decimal MinimumScore { get; set; } = 60;
}