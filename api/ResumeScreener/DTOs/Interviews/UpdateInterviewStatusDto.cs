using System.ComponentModel.DataAnnotations;

namespace ResumeScreener.DTOs.Interviews;

public class UpdateInterviewStatusDto
{
    [Required]
    [MaxLength(30)]
    public string Status { get; set; } =
        string.Empty;

    public string Feedback { get; set; } =
        string.Empty;
}