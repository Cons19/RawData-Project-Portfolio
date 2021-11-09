using System;

namespace DataAccessLayer.Domain
{
    public class RatingHistory
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public string TitleId { get; set; }
        public int Rate { get; set; }

        public User User { get; set; }
        public Title Title { get; set; }
    }
}
