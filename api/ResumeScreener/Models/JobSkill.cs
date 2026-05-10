using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeScreener.Models;

[Table("JobSkills")]
public class JobSkill
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int JobId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SkillName { get; set; } =
        string.Empty;

    public bool IsRequired { get; set; } = true;

    public int Weight { get; set; } = 5;
}