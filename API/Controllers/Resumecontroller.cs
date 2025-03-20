using System.Security.Claims;
using API.Data;
using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Resumecontroller : ControllerBase
    {
        private readonly AuthDbContext _context ;
        private readonly IWebHostEnvironment _env ;
        public Resumecontroller(AuthDbContext context,IWebHostEnvironment env)
        {
            _context=context;
            _env=env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
        return BadRequest("No file uploaded.");

    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    Console.WriteLine(userId);
    if (userId == null)
    {
        Console.WriteLine("❌ Error: User ID not found in claims.");
        return Unauthorized(new { Message = "User ID claim not found. Please ensure you're sending a valid JWT token with the correct claims." });
    }

    // Generate file path
    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
    if (!Directory.Exists(uploadsFolder))
        Directory.CreateDirectory(uploadsFolder);

    var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var resume = new Resume
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        FileName = file.FileName,
        FilePath = filePath
    };

    _context.Resumes.Add(resume);
    await _context.SaveChangesAsync();

    return Ok(new { Message = "Resume uploaded successfully.", ResumeId = resume.Id });
        }

        [HttpGet]
        public IActionResult GetResumes()
        {
            var resumes = _context.Resumes.ToList();
            return Ok(resumes);
        }
    }
}
