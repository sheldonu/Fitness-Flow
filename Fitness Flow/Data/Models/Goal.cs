namespace Fitness_Flow.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Goal
{
    [Key]
    public int GoalId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } // Link to AspNetUsers

    [Required]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? TargetDate { get; set; }
    public bool IsCompleted { get; set; } = false;
}
