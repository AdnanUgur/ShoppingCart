using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartServiceLib.Service
{
    public class DeliveryCostCalculator : IDeliveryCostCalculator
    {
        //numberOfProducts-toplam ürün sayısı
        //numberOfDeliveries -farklı kategori miktarı
        //Formula is (CostPerDelivery* NumberOfDeliveries) +(CostPerProduct* NumberOfProducts) +Fixed Cost   Fixed Cost = 2.99TL
        private double costPerDelivery;
        private double costPerProduct;
        private double fixedCost = 2.99;        

        public double CalculateFor(ShoppingCart shoppingCart)
        {
            return (costPerDelivery * shoppingCart.GetNumberOfCategory()) + (costPerProduct * shoppingCart.GetNumberOfProduct()) + fixedCost;
        }
        public DeliveryCostCalculator(double _costPerDelivery, double _costPerProduct, double _fixedCost)
        {
            costPerDelivery = _costPerDelivery;
            costPerProduct = _costPerProduct;
            fixedCost = _fixedCost;
        }
    }
}
