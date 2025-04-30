using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models
{
    public class OrderItem
    {
        [Key]
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

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

    }
}
