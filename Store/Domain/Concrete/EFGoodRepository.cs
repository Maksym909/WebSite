using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;

namespace Domain.Concrete
{
    public class EFGoodRepository : IGoodRepository
    {
        //класс который оперирует на созданном ранее классе EFDbContext 
        //и действует в качестве шлюза между бизнес-логикой приложения и базой данных.
        EFDbContext context = new EFDbContext();

        public IEnumerable<Good> Goods
        {
            get { return context.Goods; }
        }

        public void SaveGood(Good good)
        {
            if(good.Id==0)
            {
                context.Goods.Add(good);
            }
            else
            {
                Good dbEntry = context.Goods.Find(good.Id);
                if(dbEntry!=null)
                {
                    dbEntry.Name = good.Name;
                    dbEntry.Category = good.Category;
                    dbEntry.Description = good.Description;
                    dbEntry.Price = good.Price;
                    dbEntry.ImageData = good.ImageData;
                    dbEntry.ImageMimeType = good.ImageMimeType;
                }
                context.SaveChanges();
            }
        }

        public Good DeleteGood(int goodId)
        {
            Good dbEntry = context.Goods.Find(goodId);
            if(dbEntry!=null)
            {
                context.Goods.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

      
    }
}
