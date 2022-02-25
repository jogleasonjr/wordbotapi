using System;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Text.RegularExpressions;

namespace WordBot.Api
{

    public class SearchRequest
    {
        public string Guess { get; set; }
        public string Omit { get; set; }
    }
}