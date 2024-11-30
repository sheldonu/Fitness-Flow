using Microsoft.AspNetCore.Mvc.RazorPages;
using Fitness_Flow.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fitness_Flow.Pages
{
    public class LibraryModel : PageModel
    {
        private readonly ExerciseApiService _exerciseApiService;

        public LibraryModel(ExerciseApiService exerciseApiService)
        {
            _exerciseApiService = exerciseApiService;
        }

        public List<Exercise> Exercises { get; set; } = new List<Exercise>();

        public async Task OnGetAsync(string muscle = null, string type = null, string difficulty = null)
        {
            Exercises = await _exerciseApiService.GetExercisesAsync(muscle, type, difficulty);
        }
    }
}
