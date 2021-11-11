using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Domain
{
    public class SimilarTitle
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartYear { get; set; }
        public string Genre { get; set; }
    }
}
