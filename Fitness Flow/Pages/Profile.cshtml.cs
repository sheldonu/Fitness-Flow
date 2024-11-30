using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fitness_Flow.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fitness_Flow.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public UserProfile? UserProfile { get; set; }

        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            UserProfile = await _context.UserProfile.FindAsync(userId);

            if (UserProfile == null)
            {
                // Initialize a new profile if it doesn't exist
                UserProfile = new UserProfile { UserId = userId };
                _context.UserProfile.Add(UserProfile);
                await _context.SaveChangesAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            var userProfile = await _context.UserProfile.FindAsync(userId);

            if (userProfile != null)
            {
                // Update the existing profile with new values
                userProfile.FirstName = UserProfile.FirstName;
                userProfile.LastName = UserProfile.LastName;
                userProfile.Age = UserProfile.Age;
                userProfile.Weight = UserProfile.Weight;
                userProfile.Height = UserProfile.Height;
                userProfile.Gender = UserProfile.Gender;

                await _context.SaveChangesAsync();
                SuccessMessage = "Your profile has been updated successfully!";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var userId = _userManager.GetUserId(User);
            var userProfile = await _context.UserProfile.FindAsync(userId);

            if (userProfile != null)
            {
                _context.UserProfile.Remove(userProfile);
                await _context.SaveChangesAsync();
                SuccessMessage = "Your profile has been deleted.";
            }

            return RedirectToPage("/Index");
        }
    }
}
