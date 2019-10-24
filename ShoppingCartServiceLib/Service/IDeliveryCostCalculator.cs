using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartServiceLib.Service
{
    public interface IDeliveryCostCalculator
    {
        double CalculateFor(ShoppingCart shoppingCart);
    }
}
