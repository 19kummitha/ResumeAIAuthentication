using API.Data;
using API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController(AuthDbContext context) : ControllerBase
    {private readonly AuthDbContext _context = context;

        [HttpPost("match")]
        public IActionResult MatchResumesToJob([FromBody] MatchingResult matchRequest)
        {
            var resume = _context.Resumes.Find(matchRequest.ResumeId);
            var jobDescription = _context.JobDescriptions.Find(matchRequest.JobDescriptionId);

            if (resume == null || jobDescription == null)
                return NotFound("Resume or Job Description not found.");

            // Read resume text from the saved file
            string resumeText = System.IO.File.ReadAllText(resume.FilePath);
            string jobText = jobDescription.Description;

            double matchPercentage = resumeText.Contains(jobText) ? 100 : 50; // Simplified logic

            matchRequest.MatchPercentage = (decimal)matchPercentage;

            _context.MatchingResults.Add(matchRequest);
            _context.SaveChanges();

            return Ok(new { MatchPercentage = matchPercentage });
        }
}}
