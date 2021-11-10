using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Domain
{
    // For function find_person_by_profession
    public class FindPersonByProfession
    {
        public string Name { get; set; }
    }
}