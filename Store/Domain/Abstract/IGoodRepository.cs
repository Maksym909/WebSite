using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IGoodRepository
    {
        IEnumerable<Good> Goods { get; }
        void SaveGood(Good good);
        Good DeleteGood(int goodId);
    }
}
