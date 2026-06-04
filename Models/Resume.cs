namespace CVAnalyzer.Models;

public class Resume
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string AnalysisResult { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}