using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fitness_Flow.Services
{
    public class ExerciseApiService
    {
        private readonly HttpClient _httpClient;

        public ExerciseApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.api-ninjas.com/v1/");
        }

        public async Task<List<Exercise>> GetExercisesAsync(string muscle = null, string type = null, string difficulty = null)
        {
            var query = $"exercises?muscle={muscle}&type={type}&difficulty={difficulty}";
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", "xWuNKMT5dud0VAf7n9MOUg==fTRvbAAJKLQJDR8u");

            var response = await _httpClient.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Exercise>>(json);
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }
        }
    }

    public class Exercise
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Muscle { get; set; }
        public string Equipment { get; set; }
        public string Difficulty { get; set; }
        public string Instructions { get; set; }
    }
}
