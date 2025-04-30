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
    public class OrderResponse
    {
        public Guid OrderId { get; set; }

        public string? OrderNumber { get; set; }

        public string? CustomerName { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();

        public OrderUpdateRequest ToOrderUpdateRequest()
        {
            return new OrderUpdateRequest
            {
                OrderId = OrderId,
                OrderNumber = OrderNumber,
                CustomerName = CustomerName,
                OrderDate = OrderDate,
                TotalAmount = TotalAmount
            };
        }
    }

    public static class OrderExtensions
    {
        public static OrderResponse ToOrderResponse(this Order order)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount
            };
        }

        public static List<OrderResponse> ToOrderResponseList(this List<Order> orders)
        {
            List<OrderResponse> orderResponseList = new List<OrderResponse>();
            foreach (Order order in orders)
            {
                orderResponseList.Add(order.ToOrderResponse());
            }
            return orderResponseList;
        }
    }
}
