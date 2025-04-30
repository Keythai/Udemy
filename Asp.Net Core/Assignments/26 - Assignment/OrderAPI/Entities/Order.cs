using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

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

        public virtual ICollection<OrderItem>? OrderItems { get; set; }

    }
}
