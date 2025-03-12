using BLL.DTOModels;
using BLL.ServiceInterfaces;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF
{
    public class OrderService : IOrderService
    {
        private readonly WebStoreContext _context;

        public OrderService(WebStoreContext context)
        {
            _context = context;
        }

        public OrderResponseDTO GenerateOrder(int userId)
        {
            var basketItems = _context.BasketPositions.Where(b => b.UserID == userId).ToList();

            if (!basketItems.Any()) throw new InvalidOperationException("Basket is empty");

            var order = new Order
            {
                UserID = userId,
                Date = DateTime.UtcNow,
                OrderPositions = basketItems.Select(b => new OrderPosition
                {
                    ProductID = b.ProductID,
                    Amount = b.Amount,
                    Price = b.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.BasketPositions.RemoveRange(basketItems);
            _context.SaveChanges();

            return new OrderResponseDTO(
                order.ID,
                order.OrderPositions.Sum(op => op.Price * op.Amount),
                false,
                order.Date
            );
        }
        public void PayOrder(int orderId, double amountPaid)
        {
            var order = _context.Orders
                .Include(o => o.OrderPositions)
                .FirstOrDefault(o => o.ID == orderId);

            if (order == null)
                throw new Exception("Order not found");

            var totalAmount = order.OrderPositions.Sum(op => op.Price * op.Amount);

            if (amountPaid < totalAmount)
                throw new Exception("Insufficient payment");

            order.IsPaid = true;
            _context.SaveChanges();
        }

        public IEnumerable<OrderResponseDTO> GetOrders(int? orderId = null, bool? isPaid = null, string sortBy = "Date", bool ascending = true)
        {
            var query = _context.Orders.AsQueryable();

            if (orderId.HasValue)
            {
                query = query.Where(o => o.ID == orderId);
            }

            if (isPaid.HasValue)
            {
                query = query.Where(o => o.IsPaid == isPaid.Value);
            }

            if (sortBy.Equals("Date", StringComparison.OrdinalIgnoreCase))
            {
                query = ascending ? query.OrderBy(o => o.Date) : query.OrderByDescending(o => o.Date);
            }
            else if (sortBy.Equals("Value", StringComparison.OrdinalIgnoreCase))
            {
                query = ascending ? query.OrderBy(o => o.OrderPositions.Sum(op => op.Price * op.Amount)) :
                                    query.OrderByDescending(o => o.OrderPositions.Sum(op => op.Price * op.Amount));
            }

            return query.Select(o => new OrderResponseDTO(
                o.ID,
                o.OrderPositions.Sum(op => op.Price * op.Amount),
                o.IsPaid,
                o.Date)).ToList();
        }

        public IEnumerable<OrderPositionResponseDTO> GetOrderPosition(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderPositions)
                .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.ID == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            return order.OrderPositions.Select(op => new OrderPositionResponseDTO(
                op.ProductID,
                op.Product.Name,
                op.Price,
                op.Amount,
                op.Price * op.Amount
                )).ToList();
        }

    }

}
