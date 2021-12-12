using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.ViewModels
{
    public class PersonViewModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string BirthYear { get; set; }
        public string DeathYear { get; set; }
        public double? Rating { get; set; }
    }
}
