# 🧠 AI-Powered CV Analyzer (.NET + Ollama + LLaMA 3)

An intelligent resume analysis system built with **ASP.NET Core Razor Pages** that allows users to upload their CV (PDF/DOCX) and receive AI-generated feedback including score, skills, strengths, weaknesses, and improvement suggestions.

---

## 🚀 Features

- 📄 Upload CV (PDF / DOCX)
- 🧠 Extract text from resume automatically
- 🤖 AI-powered CV analysis using LLaMA 3 (Ollama)
- 📊 CV scoring system (0–100)
- 🧾 Extract skills automatically
- 💪 Identify strengths & weaknesses
- 💡 Provide career improvement suggestions
- 💾 Store analysis history in SQL Server
- 🔐 Session-based user authentication

---

## ⚙️ Tech Stack

### Backend
- ASP.NET Core Razor Pages
- C#

### AI Integration
- Ollama (Local LLM Server)
- LLaMA 3 Model
- HttpClient API integration
- Prompt Engineering (JSON structured output)

### File Processing
- PdfPig (PDF text extraction)
- OpenXML SDK (DOCX text extraction)

### Database
- SQL Server
- ADO.NET (SqlConnection, SqlCommand)

---

## 🧠 System Architecture

```
User Uploads CV
      ↓
UploadModel (Razor Page)
      ↓
File saved to wwwroot/uploads
      ↓
CvTextExtractor (PDF / DOCX parsing)
      ↓
AiService (Ollama LLM call)
      ↓
AI returns JSON response
      ↓
Parse JSON in .NET
      ↓
Store result in SQL Server
      ↓
Display result in UI
```

---

## 📁 Folder Structure

```
CVAnalyzer/
│
├── Pages/
│   ├── Upload.cshtml
│   ├── Upload.cshtml.cs
│   ├── Login.cshtml
│   ├── Index.cshtml
│
├── Services/
│   ├── AiService.cs
│   ├── CvTextExtractor.cs
│
├── wwwroot/
│   ├── uploads/          # Uploaded CV files stored here
│   ├── css/
│   ├── js/
│
├── Models/
│   ├── User.cs
│   ├── Resume.cs
│
├── Data/
│   ├── Database.cs       # DB connection helper (if used)
│
├── appsettings.json
├── Program.cs
├── CVAnalyzer.csproj
└── README.md
```

---

## 🧠 How AI Works in This Project

1. CV text is extracted from file
2. Text is sent to Ollama API:

```
http://localhost:11434/api/generate
```

3. Prompt forces structured output:

```json
{
  "score": 85,
  "skills": ["C#", "ASP.NET", "SQL"],
  "strengths": ["Problem solving"],
  "weaknesses": ["Lack of cloud experience"],
  "suggestions": ["Learn Azure", "Improve system design"]
}
```

4. Response is parsed and displayed in UI

---

## 🗄️ Database Table (Resumes)

```sql
CREATE TABLE Resumes (
    Id INT PRIMARY KEY IDENTITY,
    UserId NVARCHAR(100),
    FileName NVARCHAR(255),
    FilePath NVARCHAR(255),
    AnalysisResult NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

---

## 🔐 Key Concepts Used

- File Upload Handling
- Dependency Injection (Services)
- AI Prompt Engineering
- JSON Parsing in C#
- REST API Integration
- Session Authentication
- Layered Architecture

---



## ⭐ If you like this project

Give it a star ⭐ and feel free to contribute or improve it.
