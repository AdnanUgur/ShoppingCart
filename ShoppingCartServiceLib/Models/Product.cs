using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartServiceLib.Models
{
    public class Product
    {
        private string name;
        private Category category;
        private double price;
        public Product(string _name, double _price,Category _category)
        {
            name = _name;
            category = _category;
            price = _price;
        }
        public string GetName() { return name; }
        public Category GetCategory() { return category; }
        public double GetPrice() { return price; }
    }
}
