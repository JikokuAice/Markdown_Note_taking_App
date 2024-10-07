using Markdown_Note_taking_App.Model;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Westwind.AspNetCore.Markdown;

namespace Markdown_Note_taking_App.Controller {
  [Route("api/[controller]")]
  [ApiController]
  public class MarkdownController : ControllerBase {
    private readonly IConfiguration _configuration;

    public MarkdownController(IConfiguration configuration) {
      _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> UploadMarkdown([FromForm] IFormFile file) {
      // Check if the uploaded file is a Markdown (.md) file
      FileInfo fileInfo = new FileInfo(file.FileName);

      if (fileInfo.Extension != ".md") {
        return BadRequest("Only accepts .md file");
      }

      // path to save file
      var path = Path.Combine(
          "C:\\Users\\Ayush\\source\\repos\\Markdown Note-taking App\\Markdown Note-taking App\\post\\",
          fileInfo.Name);

      // saving file in given folder and must include filename so we use
      // path.combine earlier
      using (var stream = new FileStream(path, FileMode.Create)) {
        await file.CopyToAsync(stream);
      }

      return Ok(new { success = $"{fileInfo.Name} saved sucessfully" });
    }

    [HttpGet("files")]
    public IActionResult MarkdownFiles() {
      try {
        List<string> data = new List<string>();

        string[] files = Directory.GetFiles(
            "C:\\Users\\Ayush\\source\\repos\\Markdown Note-taking App\\Markdown Note-taking App\\post\\",
            "*.md");
        foreach (string file in files) {
          var fileName = file.Split('\\').Last();
          System.Diagnostics.Debug.WriteLine(fileName);

          data.Add(fileName);
        }
        return Ok(new { SavedNotes = data });
      } catch (Exception ex) {
        return BadRequest(ex.ToString());
      }
    }

    [HttpGet("{filename}")]
    public IActionResult Get([FromRoute] string filename) {
      if (string.IsNullOrEmpty(filename))
        return BadRequest("Pleaase give file name in route");
      var path = Path.Combine(
          "C:\\Users\\Ayush\\source\\repos\\Markdown Note-taking App\\Markdown Note-taking App\\post\\",
          filename);

      try {
        var html = Markdown.ParseFromFile(path);

        return Ok(html);

      } catch (Exception ex) {
        return BadRequest("Exception occured! please use correct file name");
      }
    }

    [HttpPost("CheckGrammer/{filename}")]
    public async Task<IActionResult> CheckGrammer([FromRoute] string filename) {
      if (string.IsNullOrEmpty(filename))
        return BadRequest("Pleaase give file name in route");
      else if (!filename.Contains('.')) {
        return BadRequest("please provide .md file extension");
      } else if (filename.Split('.')[1] != "md")
        return BadRequest("accepts markdown file only");

      var path = Path.Combine(
          "C:\\Users\\Ayush\\source\\repos\\Markdown Note-taking App\\Markdown Note-taking App\\post\\",
          filename);

      try {
        string content = System.IO.File.ReadAllText(path);
        content.Contains("bash");
        for (int i = 0; i < content.Length; i++) {
          if (content[i] == '#' || content[i] == '-' || content[i] == '[' ||
              content[i] == ']' || content[i] == '(' || content[i] == ')' ||
              content[i] == '`') {
            content = content.Replace(content[i], ' ');
          }
        }

        var client = new HttpClient();

        string key = _configuration["GApiKey"];

        System.Diagnostics.Debug.WriteLine(key);

        var response = await client.PostAsJsonAsync(
            "https://api.sapling.ai/api/v1/edits",
            new checkGrammer(key, content, "test_session"));

        var responseData =
            await response.Content.ReadFromJsonAsync<GrammerCheckResponse>();

        return Ok(responseData);
      } catch (Exception e) {
        return BadRequest("please use correct .md (Markdown) !!file name!!");
      }
    }
  }
