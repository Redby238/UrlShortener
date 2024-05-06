using Microsoft.AspNetCore.Mvc;

namespace UrlShortener_RightOne_.Models
{
    public class Click
    {
        public int Id { get; set; }
        public DateTime ClickedAt { get; set; }
        public string IpAddress { get; set; }
    }
}
