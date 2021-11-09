using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Domain
{
    public class BookmarkPerson
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PersonId { get; set; }
        public User User { get; set; }
        public Person Person { get; set; }

        public override string ToString()
        {
            return $"UserId = {UserId}, TitleId = {PersonId}";
        }
    }
}
