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
    public class OrderAddRequest
    {
        [Required(ErrorMessage = "Order Number cannot be blank")]
        [RegularExpression(@"^(?i)ORD_\d{4}_\d+$\r\n", ErrorMessage = "The Order number should begin with 'ORD' followed by an underscore (_) and a sequential number.")]
        public string? OrderNumber { get; set; }

        [Required(ErrorMessage = "Customer Name canont be blank")]
        [StringLength(50, ErrorMessage = "Customer name cannot exceed 50 characters.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Order Date cannot be blank")]
        public DateTime OrderDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The value must be a positive number.")]
        [Column(TypeName = "decimal")]
        public decimal TotalAmount { get; set; }

        public List<OrderItemAddRequest>? OrderItems { get; set; } = new List<OrderItemAddRequest>();
        public Order ToOrder()
        {
            return new Order
            {
                OrderNumber = OrderNumber,
                CustomerName = CustomerName,
                OrderDate = OrderDate,
                TotalAmount = TotalAmount
            };
        }
    }
}
