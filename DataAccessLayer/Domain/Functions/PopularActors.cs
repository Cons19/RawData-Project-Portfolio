using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Domain
{
    public class PopularActors
    {
        public string Name { get; set; }
        public double Rating { get; set; }
    }
}
