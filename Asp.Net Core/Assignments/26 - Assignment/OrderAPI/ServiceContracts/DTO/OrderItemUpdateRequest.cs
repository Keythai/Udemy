using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderAPI.Models;

namespace ServiceContracts.DTO
{
    public class OrderItemUpdateRequest
    {
        public Guid OrderItemId { get; set; }

        [Required(ErrorMessage = "Order ID cannot be blank")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "Product Name cannot be blank")]
        [StringLength(50, ErrorMessage = "Product name cannot exceed 50 characters.")]
        public string? ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive integer.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The value must be a positive number.")]
        [Column(TypeName = "decimal")]
        public decimal UnitPrice;

        [Range(0, double.MaxValue, ErrorMessage = "The value must be a positive number.")]
        [Column(TypeName = "decimal")]
        public decimal TotalPrice;

        public OrderItem ToOrderItem()
        {
            return new OrderItem
            {
                OrderItemId = OrderItemId,
                OrderId = OrderId,
                ProductName = ProductName,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                TotalPrice = TotalPrice
            };
        }
    }
}
