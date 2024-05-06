using Microsoft.AspNetCore.Mvc;

namespace UrlShortener_RightOne_.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Clicks { get; set; }
        public List<Click> ClicksList { get; set; }
    }
}
