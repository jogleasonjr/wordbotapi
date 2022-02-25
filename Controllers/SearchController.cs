using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WordBot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;
    private List<WordSource> Sources { get; set; }


    public SearchController(ILogger<SearchController> logger)
    {
        _logger = logger;

    }

    [HttpGet(Name = "Search")]
    public async Task<string> Get([FromQuery] string guess, [FromQuery] string? omit = "")
    {
        if (Sources == null)
        {
            Sources = new List<WordSource>();
            Sources.Add(await WordSource.Create("https://raw.githubusercontent.com/dwyl/english-words/master/words_alpha.txt", "English Word List 1"));
            Sources.Add(await WordSource.Create("https://raw.githubusercontent.com/first20hours/google-10000-english/master/google-10000-english-no-swears.txt", "Google 10,000 Common English Words"));

        }

        var sb = new StringBuilder();

        foreach (var source in Sources)
        {
            sb.AppendLine($"{source.Title} [{source.Words.Length:n0} words]");
            var results = source.Search(guess, omit);

            for (int i = 0; i < results.Length; i++)
            {
                sb.AppendLine($"{(i + 1).ToString().PadLeft(2)}. {results[i]}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    [HttpPost(Name = "Search")]
    public async Task<string> Post(SearchRequest request)
    {
        return await Get(request.Guess, request.Omit);
    }


    [HttpGet("telegram")]
    public async Task<string> Telegram(string search)
    {
        var parts = search.Split(',');
        var guess = parts[0].Replace('.', ' ');
        var omit = parts.Length > 1 ? parts[1] : string.Empty;

        return await Get(guess, omit);
    }
}
