using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using CVAnalyzer.Services;

namespace CVAnalyzer.Pages
{
    public class UploadModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly CvTextExtractor _extractor;
        private readonly AiService _aiService;

        public UploadModel(
            IConfiguration config,
            CvTextExtractor extractor,
            AiService aiService)
        {
            _config = config;
            _extractor = extractor;
            _aiService = aiService;
        }

        [BindProperty]
        public IFormFile CvFile { get; set; }

        public string Message { get; set; } = "";
        public string Error { get; set; } = "";

        // FINAL UI DATA
        public string RawJson { get; set; } = "";

        public int Score { get; set; }
        public List<string> Skills { get; set; } = new();
        public List<string> Strengths { get; set; } = new();
        public List<string> Weaknesses { get; set; } = new();
        public List<string> Suggestions { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            if (CvFile == null)
            {
                Error = "Please select a file";
                return Page();
            }

            string fileName = Path.GetFileName(CvFile.FileName);

            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads"
            );

            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await CvFile.CopyToAsync(stream);
            }

            // Extract CV text
            string extractedText = _extractor.ExtractText(filePath);

            // AI CALL
            var aiResult = await _aiService.AnalyzeCv(extractedText);

            RawJson = aiResult;

            // CLEAN JSON PARSING
            try
            {
                var start = aiResult.IndexOf('{');
                var end = aiResult.LastIndexOf('}');

                var cleanJson = aiResult.Substring(start, end - start + 1);

                var obj = JsonSerializer.Deserialize<JsonElement>(cleanJson);

                Score = obj.GetProperty("score").GetInt32();

                Skills = obj.GetProperty("skills")
                    .EnumerateArray()
                    .Select(x => x.GetString())
                    .ToList();

                Strengths = obj.GetProperty("strengths")
                    .EnumerateArray()
                    .Select(x => x.GetString())
                    .ToList();

                Weaknesses = obj.GetProperty("weaknesses")
                    .EnumerateArray()
                    .Select(x => x.GetString())
                    .ToList();

                Suggestions = obj.GetProperty("suggestions")
                    .EnumerateArray()
                    .Select(x => x.GetString())
                    .ToList();
            }
            catch
            {
                Error = "Failed to parse AI response";
            }

            // SAVE DB
            string cs = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                string query = @" USE CVAnalyzerDb;
                    INSERT INTO Resumes
                    (UserId, FileName, FilePath, AnalysisResult)
                    VALUES
                    (@UserId, @FileName, @FilePath, @AnalysisResult)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@FileName", fileName);
                    cmd.Parameters.AddWithValue("@FilePath", "/uploads/" + fileName);
                    cmd.Parameters.AddWithValue("@AnalysisResult", aiResult);

                    cmd.ExecuteNonQuery();
                }
            }

            Message = "CV analyzed successfully 🚀";

            return Page();
        }
    }
}