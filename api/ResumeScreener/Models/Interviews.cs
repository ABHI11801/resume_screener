using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeScreener.Models;

[Table("Interviews")]
public class Interview
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ResumeId { get; set; }

    [Required]
    public int JobId { get; set; }

    [Required]
    public DateTime InterviewDate { get; set; }

    [MaxLength(30)]
    public string Mode { get; set; } =
        "Online";

    public string MeetingLink { get; set; } =
        string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } =
        "Scheduled";

    public string Feedback { get; set; } =
        string.Empty;

    public DateTime CreatedAt { get; set; } =
        DateTime.UtcNow;
}