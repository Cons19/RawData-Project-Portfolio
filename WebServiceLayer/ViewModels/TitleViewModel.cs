using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.ViewModels
{
    public class TitleViewModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string? TitleType { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? OriginalTitle { get; set; }
        public bool? IsAdult { get; set; }
        public string? StartYear { get; set; }
        public string? EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }
        public string? Poster { get; set; }
        public string? Awards { get; set; }
        public string? Plot { get; set; }
        public int? Rating { get; set; }
    }

    public class SearchTitleViewModel
    {
        public string Id { get; set; }
        public string? PrimaryTitle { get; set; }
    }

    public class RateTitleViewModel
    {
        public int UserId { get; set; }
        public string TitleId { get; set; }
        public int Rating { get; set; }
    }
}
