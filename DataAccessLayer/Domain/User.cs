using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IList<BookmarkTitle> BookmarkTitles { get; set; }
        public IList<BookmarkPerson> BookmarkPersons { get; set; }
        public IList<RatingHistory> RatingHistories { get; set; }

        public override string ToString()
        {
            return $"Id = {Id}, Name = {Name}, Email = {Email}";
        }
    }
}