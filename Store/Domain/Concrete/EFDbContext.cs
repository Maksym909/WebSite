using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Entities;

namespace Domain.Concrete
{
    //Associate Goods with our DB
    public class EFDbContext : DbContext
    {
        public DbSet<Good> Goods { get; set; }
    }
}
