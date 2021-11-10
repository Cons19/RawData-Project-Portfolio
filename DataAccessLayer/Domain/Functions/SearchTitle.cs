using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Domain
{
    // For function search_string
    public class SearchTitle
    {
        public string Id { get; set; }
        public string PrimaryTitle { get; set; }
    }
}
