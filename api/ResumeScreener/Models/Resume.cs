using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeScreener.Models;

[Table("Resumes")]
public class Resume
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int JobId { get; set; }

    [Required]
    [MaxLength(100)]
    public string CandidateName { get; set; } =
        string.Empty;

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } =
        string.Empty;

    [Required]
    public string ResumeFilePath { get; set; } =
        string.Empty;

    public string ParsedText { get; set; } =
        string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } =
        "Uploaded";

    public DateTime UploadDate { get; set; } =
        DateTime.UtcNow;
}