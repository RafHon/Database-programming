using BLL.DTOModels;
using BLL.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF.Services
{
    public class OrderService : IOrderService
    {
        private readonly WebstoreContext _context;

        public OrderService(WebstoreContext context)
        {
            _context = context;
        }
        public IEnumerable<OrderPositionResponseDTO> GetOrderDetails(int orderID)
        {
            var order = _context.Orders
            .Include(o => o.OrderPositions)
            .ThenInclude(op => op.Product)
            .FirstOrDefault(o => o.ID == orderID);

            return order.OrderPositions.Select(op => new OrderPositionResponseDTO(op.Product.Name, op.Price, op.Amount)).ToList();
        }

        public IEnumerable<OrderResponseDTO> GetOrders(string sortBy, bool ascending, int filterID, bool filterPaid)
        {
            var query = _context.Orders
                .Include(o => o.OrderPositions)
                .ThenInclude(op => op.Product)
                .AsQueryable();

            if (filterID > 0)
                query = query.Where(o => o.ID == filterID);

            if (filterPaid)
                query = query.Where(o => o.isPaid == true);

            var orderQuery = query.Select(o => new
            {
                Order = o,
                TotalValue = o.OrderPositions.Sum(op => op.Price * op.Amount)
            });

            orderQuery = sortBy switch
            {
                "id" => ascending ? orderQuery.OrderBy(o => o.Order.ID) : orderQuery.OrderByDescending(o => o.Order.ID),
                "value" => ascending ? orderQuery.OrderBy(o => o.TotalValue) : orderQuery.OrderByDescending(o => o.TotalValue),
                "date" => ascending ? orderQuery.OrderBy(o => o.Order.Date) : orderQuery.OrderByDescending(o => o.Order.Date),
                _ => orderQuery
            };

            return orderQuery.Select(o => new OrderResponseDTO(
                o.Order.ID,
                o.TotalValue,
                o.Order.isPaid,
                o.Order.Date
            )).ToList();
        }


        private double CalculateOrderTotal(int orderID)
        {
            var orderPositions = _context.OrderPositions.Where(op => op.OrderID == orderID).ToList();
            return orderPositions.Sum(op => op.Price * op.Amount);
        }

        public void PayOrder(int orderID, double amount)
        {
            var order = _context.Orders.FirstOrDefault(o => o.ID == orderID);

            if (order.isPaid)
                throw new InvalidOperationException("Order has already been paid");


            var totalAmount = CalculateOrderTotal(orderID);

            if (amount != order.OrderPositions.First(op=>op.OrderID==orderID).Price)
                throw new InvalidOperationException("Payment amount is not enough");

            order.isPaid = true;
            _context.SaveChanges();

        }
    }
}
