using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.ViewModels
{
    public class RatingHistoryViewModel
    {
        public int UserId { get; set; }
        public string TitleId { get; set; }
        public int Rate { get; set; }
    }
}
