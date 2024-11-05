namespace Fitness_Flow.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Progress
{
    [Key]
    public int ProgressId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } // Link to AspNetUsers

    public DateTime DateLogged { get; set; } = DateTime.Now;
    public decimal Weight { get; set; }
    public string Notes { get; set; }
}
