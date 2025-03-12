using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IOrderService
    {
        OrderResponseDTO GenerateOrder(int userId);

        void PayOrder(int orderId, double amountPaid);

        IEnumerable<OrderResponseDTO> GetOrders(
            int? orderId = null,
            bool? isPaid = null,
            string? sortBy = "Date",
            bool ascending = true
        );

        IEnumerable<OrderPositionResponseDTO> GetOrderPosition(int orderId);
    }
}
