using ShoppingCartServiceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ShoppingCartServiceLib.Service
{
    public class ShoppingCart : IShoppingCartService
    {
        public Cart Cart = new Cart();
        private static Dictionary<Category, int> categoryOfProduct = new Dictionary<Category, int>();
        private static int numberOfProduct=0;
        List<KeyValuePair<Category, double>> maxDiscount = new List<KeyValuePair<Category, double>>();
        static Coupon maxDiscountCoupon;
        #region Singelton/CommentOut
        //private static ShoppingCart Instance = null;
        //public static ShoppingCart GetInstance
        //{
        //    get
        //    {
        //        if (Instance == null)
        //            Instance = new ShoppingCart();
        //        return Instance;
        //    }
        //}
        #endregion
        #region CartResponsibilty
        public void AddItem(Product product, int piece)
        {
            var flag = 0;
            SetTotalAmount(product.GetPrice()*piece);
            foreach (var item in Cart.products.ToArray())
            {
                if (item.Key.Equals(product))
                {
                    flag = 1;
                    var i= item.Value;
                    Cart.products.Remove(new KeyValuePair<Product, int>(item.Key,item.Value));
                    var newEntry = new KeyValuePair<Product, int>(product, piece + i);
                    Cart.products.Add(newEntry);
                    numberOfProduct += piece;
                    break;
                }
            }
            if(flag != 1)
            {
                numberOfProduct += piece;
                var prodi = new KeyValuePair<Product, int>(product, piece);
                Cart.products.Add(prodi);
            }
            GetCategoryProductNumber(product,piece);
        }
        public void ApplyCoupon(Coupon coupon)
        {
            Cart.coupons.Add(coupon);
        }
        public void ApplyDiscounts(params Campaign[] discounts)
        {
            Cart.campaigns.AddRange(discounts);
        }

        public double GetCampaignDiscount() //category, sayı, adet, (oran/para) 
        {
            int numberOfCategoryProduct=0;
            double totalOfCategoryProdcutPrice = 0;            
            double topOfDiscount = 0;
            double temp = 0;
            Category categoryTemp = null;

            maxDiscount.Clear();
            foreach (var item in Cart.campaigns.GroupBy(u => u.CategoryName).Select(grp => grp.ToList()).ToList())
            {
                foreach(var item2 in item)
                { 
                    if (!item2.CategoryName.Equals(categoryTemp))  //yeni kategori var demek
                    {
                        topOfDiscount = 0;
                        temp = 0;
                        maxDiscount.Add(new KeyValuePair<Category, double>(item2.CategoryName, 0));
                    }        
                    foreach (var product in Cart.products)
                    {
                        if (product.Key.GetCategory() == item2.CategoryName)
                        {
                            numberOfCategoryProduct += product.Value;
                            totalOfCategoryProdcutPrice += product.Key.GetPrice() * product.Value;
                        }
                    }
                    if (numberOfCategoryProduct >= item2.ItemCount)
                    {
                        if (item2.Type == DiscountType.Amount)
                            temp = item2.Discount;
                        else if (item2.Type == DiscountType.Rate)
                            temp = (item2.Discount * totalOfCategoryProdcutPrice) / 100.0;
                    }

                    if (temp > topOfDiscount)
                    {
                        foreach (var dis in maxDiscount.ToList())
                        {
                            if (dis.Key == item2.CategoryName)
                            {
                                if (temp >= dis.Value)
                                {
                                    var i = temp;
                                    maxDiscount.Remove(new KeyValuePair<Category, double>(dis.Key, dis.Value));
                                    maxDiscount.Add(new KeyValuePair<Category, double>(dis.Key, temp));
                                }
                            }
                        }
                        topOfDiscount = temp;
                    }
                    categoryTemp = item2.CategoryName;

                    numberOfCategoryProduct = 0;
                    totalOfCategoryProdcutPrice = 0;
                }
            }
            return maxDiscount.Sum(t => t.Value);
        }
        public double GetCouponDiscount() //min tutar , sayı,(oran/para)
        {
            double topOfDiscount = 0;
            double temp = 0;
            var tempPrice = Cart.GetAmount() - GetCampaignDiscount();

            foreach (var item in Cart.coupons)
            {
                if (tempPrice >= item.MinimumLimit)
                {
                    if (item.Type == DiscountType.Amount)
                        temp = item.Discount;
                    else if (item.Type == DiscountType.Rate)
                        temp = (item.Discount * tempPrice) / 100.0;
                }
                if (temp > topOfDiscount)
                {
                    topOfDiscount = temp;
                    maxDiscountCoupon = item;
                }                    
            }            
            return topOfDiscount;
        }
        public double GetDeliveryCost() //Error costPerDelivery/costPerProduct/fixedCost gerek vardı. Zaten bunu DeliveryCostCaltulator yapıyor.
        {
            Console.WriteLine("ERROR!");
            return 0;
        } 
        public double GetTotalAmountAfterDiscounts()
        {
            return Cart.GetAmount() - GetCampaignDiscount() - GetCouponDiscount();
        }
        public void Print()
        {
            var maxCampaign = GetCampaignDiscount();
            GetCouponDiscount();

            var maxCoupon = 0.0;            
            foreach (var item in Cart.products)
            {
                var category = item.Key.GetCategory();

                if (maxDiscountCoupon.Type == DiscountType.Amount)
                    maxCoupon = maxDiscountCoupon.Discount+ (item.Key.GetPrice() * item.Value);
                else if (maxDiscountCoupon.Type == DiscountType.Rate)
                    maxCoupon =Math.Abs((((maxDiscountCoupon.Discount)* item.Key.GetPrice() * item.Value) / 100.0) - (((maxDiscountCoupon.Discount * maxCampaign)/100.0)/Convert.ToDouble(categoryOfProduct.Count)));
                Console.WriteLine("CategoryName:{0}, ProductName: {1}, Quantity: {2}, Unit Price: {3:N}TL, Total Price: {4:N}TL, Total Discount: (CampaignDiscount:){5:N}TL ,(CouponDiscount:){6:N}TL",
                                category.name, item.Key.GetName(), item.Value, item.Key.GetPrice(), item.Key.GetPrice() * item.Value,
                                item.Value*(GetCategoryDiscount(category)/Convert.ToDouble(categoryOfProduct[item.Key.GetCategory()]))
                                , maxCoupon);
            }
        }
        #endregion
        #region helper
        private double GetCategoryDiscount(Category category)
        {
            foreach (var item2 in maxDiscount)
            {
                if (item2.Key == category)
                    return item2.Value;
            }
            return 0.0;
        }
        private void GetCategoryProductNumber(Product product,int piece)
        {
            if (categoryOfProduct.ContainsKey(product.GetCategory()))
                categoryOfProduct[product.GetCategory()] += piece;
            else
                categoryOfProduct.Add(product.GetCategory(), piece);
            
        }
        private void SetTotalAmount(double price) { Cart.SetAmount(price); }
        public double GetAmount() { return Cart.GetAmount(); }
        public int GetCampaignNumber() { return Cart.campaigns.Count; }
        public int GetCouponNumber() { return Cart.coupons.Count; }
        public int GetNumberOfProduct() { return numberOfProduct; }
        public int GetNumberOfCategory() { return categoryOfProduct.Count; }
        #endregion 
    }
}
