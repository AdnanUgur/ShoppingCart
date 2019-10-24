using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartServiceLib.Models
{
    public class Coupon
    {
        public double MinimumLimit { get; set; }
        public int Discount { get; set; }
        public DiscountType Type { get; set; }
        public Coupon(double _minimumLimit,int _discount, DiscountType _type)
        {
            MinimumLimit = _minimumLimit;
            Discount = _discount;
            Type = _type;
        }
    }
}
