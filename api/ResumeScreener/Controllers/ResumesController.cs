using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScreener.Data;
using ResumeScreener.DTOs.Resumes;
using ResumeScreener.Models;
using ResumeScreener.Services;

namespace ResumeScreener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly PdfService _pdfService;
    private readonly ScoringService _scoringService;

    public ResumesController(ApplicationDbContext context,PdfService pdfService,ScoringService scoringService)
    {
        _context = context;
        _pdfService = pdfService;
        _scoringService = scoringService;
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadResume(
        [FromForm] UploadResumeDto dto)
    {
        if (dto.ResumeFile.ContentType != "application/pdf")
        {
            return BadRequest("Only PDF files allowed");
        }

        var uploadPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads",
            "Resumes"
        );

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var fileName =
            Guid.NewGuid().ToString()
            + Path.GetExtension(dto.ResumeFile.FileName);

        var filePath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(
            filePath,
            FileMode.Create))
        {
            await dto.ResumeFile.CopyToAsync(stream);
        }

        var resume = new Resume
        {
            JobId = dto.JobId,
            CandidateName = dto.CandidateName,
            Email = dto.Email,
            ResumeFilePath =
                $"Uploads/Resumes/{fileName}",
            Status = "Uploaded"
        };

        _context.Resumes.Add(resume);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Resume uploaded successfully",
            resumeId = resume.Id
        });
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetResumes()
    {
        var resumes = _context.Resumes
            .OrderByDescending(x => x.UploadDate)
            .ToList();

        return Ok(resumes);
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetResumeById(int id)
    {
        var resume = _context.Resumes
            .FirstOrDefault(x => x.Id == id);

        if (resume == null)
        {
            return NotFound("Resume not found");
        }

        return Ok(resume);
    }


    [Authorize(Roles = "Admin,HR")]
    [HttpPost("{id}/parse")]
    public async Task<IActionResult> ParseResume(int id)
    {
        var resume = _context.Resumes
            .FirstOrDefault(x => x.Id == id);

        if (resume == null)
        {
            return NotFound("Resume not found");
        }

        var fullPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            resume.ResumeFilePath
        );

        if (!System.IO.File.Exists(fullPath))
        {
            return NotFound("Resume file missing");
        }

        string extractedText =
            _pdfService.ExtractText(fullPath);

        resume.ParsedText = extractedText;

        resume.Status = "Parsed";

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Resume parsed successfully",
            parsedCharacters = extractedText.Length
        });
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost("{id}/score")]
    public async Task<IActionResult> ScoreResume(int id)
    {

        var resume = _context.Resumes
            .FirstOrDefault(x => x.Id == id);

        if (resume == null)
        {
            return NotFound("Resume not found");
        }


        if (string.IsNullOrWhiteSpace(
            resume.ParsedText))
        {
            return BadRequest(
                "Resume must be parsed first"
            );
        }

        var jobSkills = _context.JobSkills
            .Where(x => x.JobId == resume.JobId)
            .ToList();

        if (!jobSkills.Any())
        {
            return BadRequest(
                "No skills found for this job"
            );
        }

        //calculating score
        var score = _scoringService
            .CalculateScore(
                resume,
                jobSkills
            );

        //remove old score
        var existingScore = _context.Scores
            .FirstOrDefault(x => x.ResumeId == id);

        if (existingScore != null)
        {
            _context.Scores.Remove(existingScore);
        }

        _context.Scores.Add(score);

        resume.Status = "Screened";

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Resume scored successfully",
            score = score.TotalScore,
            matchedSkills = score.MatchedSkills,
            missingSkills = score.MissingSkills,
            explanation = score.Explanation
        });
    }
}