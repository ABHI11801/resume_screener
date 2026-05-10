using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScreener.Data;
using ResumeScreener.DTOs.Interviews;
using ResumeScreener.Models;

namespace ResumeScreener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InterviewsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InterviewsController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<IActionResult> ScheduleInterview(
        ScheduleInterviewDto dto)
    {
        var resume = _context.Resumes
            .FirstOrDefault(x => x.Id == dto.ResumeId);

        if (resume == null)
        {
            return NotFound("Resume not found");
        }

        var interview = new Interview
        {
            ResumeId = dto.ResumeId,
            JobId = dto.JobId,
            InterviewDate = dto.InterviewDate,
            Mode = dto.Mode,
            MeetingLink = dto.MeetingLink,
            Status = "Scheduled"
        };

        _context.Interviews.Add(interview);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Interview scheduled successfully",
            interviewId = interview.Id
        });
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetInterviews()
    {
        var interviews = _context.Interviews
            .OrderByDescending(x => x.InterviewDate)
            .ToList();

        return Ok(interviews);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateInterviewStatusDto dto)
    {
        var interview = _context.Interviews
            .FirstOrDefault(x => x.Id == id);

        if (interview == null)
        {
            return NotFound("Interview not found");
        }

        interview.Status = dto.Status;

        interview.Feedback = dto.Feedback;

        await _context.SaveChangesAsync();

        return Ok("Interview updated successfully");
    }
}