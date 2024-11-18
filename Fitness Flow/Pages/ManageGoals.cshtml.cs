using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fitness_Flow.Data.Models;

namespace Fitness_Flow.Pages
{
    public class ManageGoalsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ManageGoalsModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public Goal NewGoal { get; set; } = new Goal();

        public List<Goal> UserGoals { get; set; } = new List<Goal>();

        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            UserGoals = _context.Goals.Where(g => g.UserId == userId).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                UserGoals = _context.Goals.Where(g => g.UserId == userId).ToList();
                return Page();
            }

            NewGoal.UserId = userId;
            _context.Goals.Add(NewGoal);
            await _context.SaveChangesAsync();
            SuccessMessage = "Your new goal has been added successfully!";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCompleteAsync(int goalId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var goal = await _context.Goals.FindAsync(goalId);
            if (goal != null && goal.UserId == userId)
            {
                goal.IsCompleted = true;
                await _context.SaveChangesAsync();
                SuccessMessage = "Goal marked as completed!";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int goalId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var goal = await _context.Goals.FindAsync(goalId);
            if (goal != null && goal.UserId == userId)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
                SuccessMessage = "Goal deleted successfully!";
            }

            return RedirectToPage();
        }
    }
}
