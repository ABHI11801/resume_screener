using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ResumeScreener.DTOs.Resumes;

public class UploadResumeDto
{
    [Required]
    public int JobId { get; set; }

    [Required]
    [MaxLength(100)]
    public string CandidateName { get; set; } =
        string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } =
        string.Empty;

    [Required]
    public IFormFile ResumeFile { get; set; } =
        default!;
}