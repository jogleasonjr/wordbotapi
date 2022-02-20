using System;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Text.RegularExpressions;

namespace WordBot.Api
{

    public class WordSource
    {
        protected static HttpClient HttpClient { get; } = new HttpClient();
        public string Url { get; }
        public string Title { get; }
        public string[] Words { get; }
        protected WordSource(string url, string title, string[] words)
        {
            Url = url;
            Title = title;
            Words = words;
        }

        public static async Task<WordSource> Create(string url, string title)
        {
            var resp = await HttpClient.GetAsync(url);
            var content = await resp.Content.ReadAsStringAsync();
            var words = content.Split(Environment.NewLine)
                .Select(w => w.ToLower().Trim())
                .Where(w => Regex.IsMatch(w, "^[a-zA-Z]+$"))
                .ToArray();

            var ws = new WordSource(url, title, words);

            return ws;
        }

        public string[] Search(string guess, string omit = "")
        {
            var filtered = Words.Where(w => w.Length == guess.Length && !omit.Any(c => w.Contains(c))).ToList();

            for (int i = 0; i < guess.Length; i++)
            {
                char c = guess[i];

                if (c == ' ') continue;

                var l = c.ToString().ToLower()[0];

                if (char.IsUpper(c))
                {
                    filtered = filtered.Where(f => f[i] == l).ToList();
                }
                else
                {
                    filtered = filtered.Where(f => f.Contains(l)).ToList();
                }
            }

            return filtered.ToArray();
        }
    }
}