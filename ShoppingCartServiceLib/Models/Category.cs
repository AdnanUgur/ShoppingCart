using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartServiceLib.Models
{
    public class Category
    {
        public string name { get; set; }

        public Category(string _name)
        {
            name = _name;
        }
    }
}
