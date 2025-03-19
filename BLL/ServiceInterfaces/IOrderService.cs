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
        IEnumerable<OrderResponseDTO> GetOrders(string sortBy, bool ascending, int filterID, bool filterPaid);
        IEnumerable<OrderPositionResponseDTO> GetOrderDetails(int orderID);
        void PayOrder(int orderID, double amount);
    }
}
