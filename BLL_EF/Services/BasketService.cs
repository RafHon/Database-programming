using BLL.ServiceInterfaces;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF.Services
{
    public class BasketService : IBasketService
    {
        private readonly WebstoreContext _context;

        public BasketService(WebstoreContext context)
        {
            _context = context;
        }

        public void AddToBasket(int userID, int productID, int amount)
        {
            var basketItem = _context.BasketPositions.FirstOrDefault(bp => bp.UserID == userID && bp.ProductID == productID);
            if (basketItem != null)
                basketItem.Amount += amount;
            else
                _context.BasketPositions.Add(new BasketPosition { UserID = userID, ProductID = productID, Amount = amount });

            _context.SaveChanges();
        }

        public void UpdateBasketItem(int userID, int productID, int amount)
        {
            var basketItem = _context.BasketPositions.FirstOrDefault(bp => bp.UserID == userID && bp.ProductID == productID);
            if (basketItem != null)
            {
                basketItem.Amount = amount;
                _context.SaveChanges();
            }
        }

        public void RemoveFromBasket(int userID, int productID)
        {
            var basketItem = _context.BasketPositions.FirstOrDefault(bp => bp.UserID == userID && bp.ProductID == productID);
            if (basketItem != null)
            {
                _context.BasketPositions.Remove(basketItem);
                _context.SaveChanges();
            }
        }

        public int GenerateOrder(int userID)
        {
            var basketItems = _context.BasketPositions.Where(bp => bp.UserID == userID).ToList();

            var order = new Order { UserID = userID, Date = DateTime.UtcNow };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in basketItems)
            {
                var product = _context.Products.Find(item.ProductID);
                _context.OrderPositions.Add(new OrderPosition { OrderID = order.ID, ProductID = item.ProductID, Amount = item.Amount, Price = product.Price * item.Amount });
            }

            _context.BasketPositions.RemoveRange(basketItems);
            _context.SaveChanges();
            return order.ID;
        }
    }
}
