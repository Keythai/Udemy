using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        [Required(ErrorMessage = "Stock symbol cannot be blank")]
        public string? StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock name cannot be blank")]
        public string? StockName { get; set; }
        [Range(typeof(DateTime), "2000-01-01", "9999-12-31", ErrorMessage = "Date must be 2000-01-01 or later.")]
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
        public uint Quantity { get; set; }
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(BuyOrderResponse)) return false;
            BuyOrderResponse order_to_compare = (BuyOrderResponse)obj;
            return BuyOrderID == order_to_compare.BuyOrderID &&
                StockSymbol == order_to_compare.StockSymbol &&
                StockName == order_to_compare.StockName &&
                DateAndTimeOfOrder == order_to_compare.DateAndTimeOfOrder &&
                Quantity == order_to_compare.Quantity &&
                Price == order_to_compare.Price &&
                TradeAmount == order_to_compare.TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
