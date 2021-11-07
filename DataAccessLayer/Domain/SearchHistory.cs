using System;

namespace DataAccessLayer.Domain
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public string SearchText { get; set; }
    }
}
