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

        var topScore =
            _context.Scores
                .OrderByDescending(x => x.TotalScore)
                .Select(x => x.TotalScore)
                .FirstOrDefault();

        return Ok(new
        {
            totalJobs,
            totalResumes,
            totalInterviews,
            topCandidateScore = topScore
        });
    }
}