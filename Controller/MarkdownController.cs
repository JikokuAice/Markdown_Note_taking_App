using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Westwind.AspNetCore.Markdown;

namespace Markdown_Note_taking_App.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class MarkdownController : ControllerBase
	{
		[HttpGet]
		public IActionResult GET()
		{
			return Ok("api works laike sasas");
		}


        [HttpPost]
        public async Task<IActionResult> UploadMarkdown([FromForm] IFormFile file)
        {
            // Check if the uploaded file is a Markdown (.md) file
            FileInfo fileInfo = new FileInfo(file.FileName);
           
            if (fileInfo.Extension != ".md")
            {
                return BadRequest("Only accepts .md file");
            }

           
           //path to save file 
            var path = Path.Combine("C:\\Users\\Ayush\\source\\repos\\Markdown Note-taking App\\Markdown Note-taking App\\post\\", fileInfo.Name);

         
            //saving file in given folder and must include filename so we use path.combine earlier
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //parse from path after our .md is daved in folder

            var html = Markdown.ParseFromFile(path);


            
            return Ok(html);
        }










    }
}
