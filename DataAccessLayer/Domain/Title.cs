using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Domain
{
    public class Title
    {
        public string Id { get; set; }
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
        public IList<BookmarkTitle> BookmarkTitles { get; set; }
        public IList<RatingHistory> RatingHistories { get; set; }

        public override string ToString()
        {
            return $"Id = {Id}, TitleType = {TitleType}, PrimaryTitle = {PrimaryTitle}, OriginalTitle = {OriginalTitle}, IsAdult = {IsAdult}, " +
                $"StartYear = {StartYear}, EndYear = {EndYear}, RunTimeMinutes = {RunTimeMinutes}, Poster = {Poster}, Awards = {Awards}, Plot = {Plot}";
        }
    }
}
