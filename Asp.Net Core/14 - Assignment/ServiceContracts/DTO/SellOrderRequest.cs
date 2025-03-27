using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class SellOrderRequest
    {
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
    }
}
