using NUnit.Framework;
using ShoppingCartServiceLib.Models;
using ShoppingCartServiceLib.Service;
using System;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ApplyProduct_Add_IncreaseProductCount()
        {
            //Arrange
            Category food = new Category("food");
            Product apple = new Product("Apple", 100.0, food);
            int piece = 1;
            ShoppingCart cart = new ShoppingCart();
            var previousCount = cart.GetNumberOfProduct();
            //Act
            cart.AddItem(apple,piece);

            //Assert
            var latestCount = cart.GetNumberOfProduct();
            Assert.IsTrue(previousCount + piece == latestCount);
        }
        [Test]
        public void ApplyCampaign_Add_IncreaseCampaignCount()
        {
            //Arrange
            ShoppingCart cart = new ShoppingCart();
            Category food = new Category("food");
            Campaign campaign = new Campaign(food, 50.5, 10, DiscountType.Rate);
            var previousCount = cart.GetCampaignNumber();
            //Act
            cart.ApplyDiscounts(campaign);

            //Assert
            var latestCount = cart.GetCampaignNumber();
            Assert.IsTrue(previousCount+1 == latestCount);
        }
        [Test]
        public void ApplyCoupon_Add_IncreaseCouponCount()
        {
            //Arrange
            Category food = new Category("food");
            Product apple = new Product("Apple", 100.0, food);
            Coupon coupon = new Coupon(100, 10, DiscountType.Rate);
            ShoppingCart cart = new ShoppingCart();
            var previousCount = cart.GetCouponNumber();
            //Act
            cart.ApplyCoupon(coupon);

            //Assert
            var latestCount = cart.GetCouponNumber();
            Assert.IsTrue(previousCount + 1 == latestCount);
        }
        [Test]
        public void AddTwoDiffrentCategoryItems_SoppingCart_CompareCartAmount()
        {
            //Arrange
            Category food = new Category("food");
            Category cleaning = new Category("cleaning");
            Product apple = new Product("Apple", 100.0, food);
            Product detergent = new Product("Detergent", 300.0, cleaning);
            ShoppingCart cart = new ShoppingCart();
            var piece = 1;
            var expectedAmount = apple.GetPrice()*piece + detergent.GetPrice()*piece ;
            
            //Act
            cart.AddItem(apple, piece);
            cart.AddItem(detergent, piece);

            //Assert
            var latestAmount = cart.GetAmount();
            Assert.IsTrue(latestAmount == expectedAmount);
        }
        [Test]
        public void CalculateOnlyCouponDiscount_SoppingCart_CompareDiscount()
        {
            //Arrange
            Category food = new Category("food");
            Category cleaning = new Category("cleaning");
            Product apple = new Product("Apple", 100.0, food);
            Product detergent = new Product("Detergent", 300.0, cleaning);
            ShoppingCart cart = new ShoppingCart();
            var piece = 1;            
            Coupon coupon = new Coupon(100, 10, DiscountType.Rate);
            var expectedAmount = ((apple.GetPrice() * piece + detergent.GetPrice() * piece)*10.0)/100.0;
            //Act
            cart.AddItem(apple, piece);
            cart.AddItem(detergent, piece);
            cart.ApplyCoupon(coupon);            

            //Assert
            var latestAmount = cart.GetCouponDiscount();
            Assert.IsTrue(latestAmount == expectedAmount);
        }
        [Test]
        public void CalculateOnlyCampaingDiscount_SoppingCart_CompareDiscount()
        {
            //Arrange
            Category food = new Category("food");
            Category cleaning = new Category("cleaning");
            Product apple = new Product("Apple", 100.0, food);
            Product detergent = new Product("Detergent", 300.0, cleaning);
            ShoppingCart cart = new ShoppingCart();
            var piece = 1;
            Campaign campaign = new Campaign(food, 25.5, 1, DiscountType.Rate);
            var expectedAmount = ((apple.GetPrice() * piece) * 25.5) / 100.0;
            //Act
            cart.AddItem(apple, piece);
            cart.AddItem(detergent, piece);
            cart.ApplyDiscounts(campaign);

            //Assert
            var latestAmount = cart.GetCampaignDiscount();
            Assert.IsTrue(latestAmount == expectedAmount);
        }
        [Test]
        public void CalculateDiffrentCategoryCampaingDiscount_SoppingCart_ReturnTotalDiscount()
        {
            //Arrange
            Category food = new Category("food");
            Category cleaning = new Category("cleaning");
            Product apple = new Product("Apple", 100.0, food);
            Product detergent = new Product("Detergent", 300.0, cleaning);
            ShoppingCart cart = new ShoppingCart();
            var piece = 1;
            Campaign campaign = new Campaign(food, 25.5, 1, DiscountType.Rate);
            Campaign campaign2 = new Campaign(cleaning, 9.0, 1, DiscountType.Rate);
            var expectedAmount = (((apple.GetPrice() * piece) * 25.5) +((detergent.GetPrice() * piece) * 9.0))/ 100.0 ;
            //Act
            cart.AddItem(apple, piece);
            cart.AddItem(detergent, piece);
            cart.ApplyDiscounts(campaign,campaign2);

            //Assert
            var latestAmount = cart.GetCampaignDiscount();
            Assert.IsTrue(latestAmount == expectedAmount);
        }
        [Test]
        public void CompareSameCategoryCampaingDiscount_SoppingCart_ReturnBiggestDiscount()
        {
            //Arrange
            Category food = new Category("food");
            Category cleaning = new Category("cleaning");
            Product apple = new Product("Apple", 100.0, food);
            Product detergent = new Product("Detergent", 300.0, cleaning);
            ShoppingCart cart = new ShoppingCart();
            var piece = 1;
            Campaign campaign = new Campaign(food, 25.5, 1, DiscountType.Rate);
            Campaign campaign2 = new Campaign(food, 9.0, 1, DiscountType.Rate);
            var expectedAmount = ((apple.GetPrice() * piece) * 25.5)/ 100.0;
            //Act
            cart.AddItem(apple, piece);
            cart.AddItem(detergent, piece);
            cart.ApplyDiscounts(campaign, campaign2);

            //Assert
            var latestAmount = cart.GetCampaignDiscount();
            Assert.IsTrue(latestAmount == expectedAmount);
        }
        [Test]
        public void DeliveryCost_SoppingCart_ReturnTrue()
        {
            //Arrange
            var piece = 1;
            const double costPerDelivery = 1.0;
            const double costPerProduct = 2.0;
            const double fixedCost = 2.99;
            Category food = new Category("food");
            Category cleaning = new Category("cleaning");
            Product apple = new Product("Apple", 100.0, food);
            Product detergent = new Product("Detergent", 300.0, cleaning);
            DeliveryCostCalculator deliveryCalculator = new DeliveryCostCalculator(costPerDelivery, costPerProduct, fixedCost);
            ShoppingCart cart = new ShoppingCart();           
           
            //Act
            cart.AddItem(apple, piece);
            cart.AddItem(detergent, piece);

            //Assert
            var expectedAmount = (costPerDelivery * cart.GetNumberOfCategory()) + (costPerProduct * cart.GetNumberOfProduct()) + fixedCost;
            var latestAmount = deliveryCalculator.CalculateFor(cart);
            Assert.IsTrue(latestAmount == expectedAmount);
        }

    }
}