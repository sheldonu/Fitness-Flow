using Fitness_Flow.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Fitness_Flow.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DashboardModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public Dictionary<string, decimal?> Averages { get; set; } = new();
        public Dictionary<string, decimal?> UserStats { get; set; } = new();
        public List<Goal>? UserGoals { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            // Fetch goals for the logged-in user
            UserGoals = await _context.Goals
                .Where(goal => goal.UserId == userId)
                .OrderBy(goal => goal.CreatedAt)
                .ToListAsync();

            var userProfile = await _context.UserProfile.FindAsync(userId);

            if (userProfile != null)
            {
                UserStats["Age"] = userProfile.Age;
                UserStats["Weight"] = userProfile.Weight;
                UserStats["Height"] = userProfile.Height;
            }

            Averages["Age"] = _context.UserProfile.Where(p => p.Age.HasValue).Average(p => (decimal?)p.Age);
            Averages["Weight"] = _context.UserProfile.Where(p => p.Weight.HasValue).Average(p => p.Weight);
            Averages["Height"] = _context.UserProfile.Where(p => p.Height.HasValue).Average(p => p.Height);


            return Page();
        }
    }

}
