using System.ComponentModel.DataAnnotations;

namespace ResumeScreener.DTOs.Interviews;

public class ScheduleInterviewDto
{
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
}