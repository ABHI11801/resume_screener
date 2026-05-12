using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScreener.Data;

namespace ResumeScreener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var totalJobs =
            _context.Jobs.Count();

        var totalResumes =
            _context.Resumes.Count();

        var totalInterviews =
            _context.Interviews.Count();

        var totalScreenedCandidates =
            _context.Scores.Count();


        var upcomingInterviews =
        (
            from interview in _context.Interviews

            join resume in _context.Resumes
                on interview.ResumeId equals resume.Id

            join job in _context.Jobs
                on interview.JobId equals job.Id

            where interview.InterviewDate >= DateTime.UtcNow

            orderby interview.InterviewDate

            select new
            {
                candidateName =
                    resume.CandidateName,

                jobTitle =
                    job.Title,

                interviewDate =
                    interview.InterviewDate,

                status =
                    interview.Status
            }
        )
        .Take(5)
        .ToList();
        return Ok(new
        {
            totalJobs,
            totalResumes,
            totalInterviews,
            totalScreenedCandidates,
            upcomingInterviews
        });
    }
}