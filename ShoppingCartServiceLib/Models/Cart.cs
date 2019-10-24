using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartServiceLib.Models
{
    public class Cart
    {
        public List<KeyValuePair<Product, int>> products { get; set; }
        public List<Campaign> campaigns { get; set; }
        public List<Coupon> coupons { get; set; }
        private double amount;
        public Cart()
        {
            products = new List<KeyValuePair<Product, int>>();
            campaigns = new List<Campaign>();
            coupons = new List<Coupon>();
            amount = 0;
        }
        public void SetAmount(double _amount) { amount += _amount; }
        public double GetAmount() { return amount; }
    }
}
