using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScreener.Data;
using ResumeScreener.DTOs.Jobs;
using ResumeScreener.Models;
using System.Security.Claims;

namespace ResumeScreener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public JobsController(ApplicationDbContext context)
    {
        _context = context;
    }



    //add job
    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<IActionResult> CreateJob(
        CreateJobDto dto)
    {
        var userEmail = User.FindFirstValue(
            ClaimTypes.Email
        );

        var user = _context.Users
            .FirstOrDefault(x => x.Email == userEmail);

        if (user == null)
        {
            return Unauthorized();
        }

        var job = new Job
        {
            Title = dto.Title,
            Department = dto.Department,
            Description = dto.Description,
            MinimumScore = dto.MinimumScore,
            CreatedByUserId = user.Id
        };

        _context.Jobs.Add(job);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Job created successfully",
            jobId = job.Id
        });
    }

    //get jobs
    [Authorize]
    [HttpGet]
    public IActionResult GetJobs()
    {
        var jobs = _context.Jobs
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return Ok(jobs);
    }

    //get job by id
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetJobById(int id)
    {
        var job = _context.Jobs
            .FirstOrDefault(x => x.Id == id);

        if (job == null)
        {
            return NotFound("Job not found");
        }

        return Ok(job);
    }

    //add skill
    [Authorize(Roles = "Admin,HR")]
    [HttpPost("skills")]
    public async Task<IActionResult> AddSkill(
        AddJobSkillDto dto)
    {
        // Check if job exists
        var job = _context.Jobs
            .FirstOrDefault(x => x.Id == dto.JobId);

        if (job == null)
        {
            return NotFound("Job not found");
        }

        var skill = new JobSkill
        {
            JobId = dto.JobId,
            SkillName = dto.SkillName,
            IsRequired = dto.IsRequired,
            Weight = dto.Weight
        };

        _context.JobSkills.Add(skill);

        await _context.SaveChangesAsync();

        return Ok("Skill added successfully");
    }

    //get skill by id
    [Authorize]
    [HttpGet("{jobId}/skills")]
    public IActionResult GetJobSkills(int jobId)
    {
        var skills = _context.JobSkills
            .Where(x => x.JobId == jobId)
            .OrderByDescending(x => x.Weight)
            .ToList();

        return Ok(skills);
    }

    //ranking
    [Authorize]
    [HttpGet("{jobId}/rankings")]
    public IActionResult GetRankings(int jobId)
    {
        var rankings = (
            from score in _context.Scores
            join resume in _context.Resumes
                on score.ResumeId equals resume.Id

            where score.JobId == jobId

            orderby score.TotalScore descending

            select new
            {
                resume.Id,
                resume.CandidateName,
                resume.Email,

                score.TotalScore,

                score.MatchedSkills,

                score.MissingSkills,

                score.Explanation
            }
        ).ToList();

        return Ok(rankings);
    }
    [Authorize]
    [HttpGet("{jobId}/candidates")]
    public IActionResult GetCandidates(
        int jobId)
    {
        var candidates =
            _context.Resumes
                .Where(x => x.JobId == jobId)
                .Select(x => new
                {
                    x.Id,

                    x.CandidateName,

                    x.Email,

                    x.Status,

                    x.ResumeFilePath
                })
                .OrderByDescending(x => x.Id)
                .ToList();

        return Ok(candidates);
    }
}