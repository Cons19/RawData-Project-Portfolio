using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class QueryString
    {
        private int _pageSize = 10;

        public const int MaxPageSize = 25;

        public int Page { get; set; } = 0;
        public int UserId { get; set; } = 0;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}