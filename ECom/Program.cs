using ShoppingCartServiceLib.Models;
using ShoppingCartServiceLib.Service;
using System;

namespace Drive
{
    class Program
    {
        static void Main(string[] args) //For test drive
        {
            Console.WriteLine("Hello Trendyol");
            const double  costPerDelivery = 1.0;
            const double costPerProduct = 2.0;
            const double fixedCost = 2.99;

            //Category Part
            Category food = new Category("food");
            Category sport = new Category("sport");
            Category device = new Category("device");
            //Product Part
            Product apple = new Product("Apple", 100.0, food);
            Product almond = new Product("Almond", 150.0, food);
            Product ball = new Product("Ball", 500.0, sport);
            Product robot = new Product("robot", 2000.0, device);

            ShoppingCart cart = new ShoppingCart();

            //Campaign Part
            Campaign campaign1 = new Campaign(food, 20.0, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(food, 50.0, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(food, 50.5, 10, DiscountType.Rate);
            Campaign campaign4 = new Campaign(sport, 25.5, 2, DiscountType.Rate);
            Campaign campaign5 = new Campaign(sport, 30.0, 2, DiscountType.Rate);
            Campaign campaign6 = new Campaign(device, 10.0, 1, DiscountType.Rate);
            //Coupon Part
            Coupon coupon1 = new Coupon(100, 10, DiscountType.Rate);
            Coupon coupon2 = new Coupon(100, 25, DiscountType.Rate);

            cart.AddItem(robot, 2);
            cart.AddItem(apple, 3);
            cart.AddItem(almond, 2);
            cart.AddItem(ball, 2);
            cart.ApplyDiscounts(campaign1, campaign2,campaign3,campaign4,campaign5, campaign6);
            cart.ApplyCoupon(coupon1);
            cart.ApplyCoupon(coupon2);
            cart.AddItem(apple,4);

            Console.WriteLine("Total price of Cart:{0:N}TL", cart.GetAmount());
            Console.WriteLine("Only Campain Discount:{0:N}TL", cart.GetCampaignDiscount());
            Console.WriteLine("Only Coupon Discount:{0:N}TL", cart.GetCouponDiscount());            
            cart.Print();
            Console.WriteLine("TotalAmountAfterDiscount:{0:N}TL", cart.GetTotalAmountAfterDiscounts());
            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(costPerDelivery, costPerProduct, fixedCost);
            Console.WriteLine("DeliveryCost:{0:N}TL", deliveryCostCalculator.CalculateFor(cart));

            Console.ReadKey();
        }
    }
}
