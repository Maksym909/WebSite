using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Good good, int quantity)
        {
            CartLine line = lineCollection.Where(g => g.Good.Id == good.Id).FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Good = good,
                    Quantity = quantity
                });
            }
            else
                line.Quantity += quantity;
        }
        public void RemoveLine(Good good)
        {
            lineCollection.RemoveAll(l => l.Good.Id == good.Id);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Good.Price * e.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
    public class CartLine
        {
            public Good Good { get; set; }
            public int Quantity { get; set; }
        }
    
}
