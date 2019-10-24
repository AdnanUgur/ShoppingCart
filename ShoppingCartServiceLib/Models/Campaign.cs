using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartServiceLib.Models
{
    public class Campaign
    {
        public Category CategoryName { get; set; }
        public double Discount { get; set; }
        public int ItemCount { get; set; }
        public DiscountType Type { get; set; }
        public Campaign(Category _categoryName, double _discount,int _itemCount,DiscountType _type)
        {
            CategoryName = _categoryName;
            Discount = _discount;
            ItemCount = _itemCount;
            Type = _type;
        }
    }
}
