using Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class SellOrderRequest : IValidatableObject, IOrderRequest
    {
        [Required(ErrorMessage = "Stock symbol cannot be blank")]
        public string? StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock name cannot be blank")]
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
        public uint Quantity { get; set; }
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public double Price { get; set; }
        public SellOrder ToSellOrder()
        {
            return new SellOrder
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price
            };
        }

        /// <summary>
        /// Model class-level validation using IValidatableObject
        /// </summary>
        /// <param name="validationContext">ValidationContext to validate</param>
        /// <returns>Returns validation errors as ValidationResult</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                results.Add(new ValidationResult("Date and time of order cannot be before 2000-01-01"));
            }
            return results;
        }
    }
}
