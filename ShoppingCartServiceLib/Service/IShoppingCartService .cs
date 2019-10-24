
using ShoppingCartServiceLib.Models;

namespace ShoppingCartServiceLib.Service
{
    public interface IShoppingCartService
    {
        void AddItem(Product product, int piece);
        void ApplyDiscounts(params Campaign[] discounts);
        void ApplyCoupon(Coupon coupon);
        void Print();
        double GetTotalAmountAfterDiscounts();
        double GetCouponDiscount();
        double GetCampaignDiscount();
        double GetDeliveryCost();      
    }
}
