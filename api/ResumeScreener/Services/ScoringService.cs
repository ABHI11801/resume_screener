using ResumeScreener.Data;
using ResumeScreener.Models;

namespace ResumeScreener.Services;

public class ScoringService
{
    private readonly ApplicationDbContext _context;

    public ScoringService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Score CalculateScore(
        Resume resume,
        List<JobSkill> jobSkills)
    {
        string resumeText =
            resume.ParsedText.ToLower();

        int totalWeight = 0;

        int matchedWeight = 0;

        List<string> matchedSkills = new();

        List<string> missingSkills = new();

        foreach (var skill in jobSkills)
        {
            totalWeight += skill.Weight;

            bool exists =
                resumeText.Contains(
                    skill.SkillName.ToLower()
                );

            if (exists)
            {
                matchedWeight += skill.Weight;

                matchedSkills.Add(skill.SkillName);
            }
            else
            {
                missingSkills.Add(skill.SkillName);
            }
        }

        decimal finalScore = 0;

        if (totalWeight > 0)
        {
            finalScore =
                ((decimal)matchedWeight /
                totalWeight) * 100;
        }

        string explanation =
            $"Matched {matchedSkills.Count} skills. " +
            $"Missing {missingSkills.Count} skills.";

        return new Score
        {
            ResumeId = resume.Id,
            JobId = resume.JobId,
            TotalScore =
                Math.Round(finalScore, 2),

            MatchedSkills =
                string.Join(", ", matchedSkills),

            MissingSkills =
                string.Join(", ", missingSkills),

            Explanation = explanation
        };
    }
}