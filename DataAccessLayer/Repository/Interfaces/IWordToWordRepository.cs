using DataAccessLayer.Domain.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IWordToWordRepository : IDisposable
    {
        IEnumerable<WordToWord> GetWordToWord(string[] words);
    }
}
