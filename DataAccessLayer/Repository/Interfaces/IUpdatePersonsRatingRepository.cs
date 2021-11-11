﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IUpdatePersonsRatingRepository : IDisposable
    {
        void UpdatePersonsRating();
        void Save();
    }
}
