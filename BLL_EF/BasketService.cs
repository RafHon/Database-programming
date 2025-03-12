using BLL.ServiceInterfaces;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF
{
    public class BasketService : IBasketService
    {
        private readonly WebStoreContext _context;

        public BasketService(WebStoreContext context)
        {
            _context = context;
        }

        public void AddToBasket(int userId, int productId, int amount)
        {
            var basketPosition = _context.BasketPositions
                .FirstOrDefault(bp => bp.UserID == userId && bp.ProductID == productId);

            if (basketPosition == null)
            {
                _context.BasketPositions.Add(new BasketPosition
                {
                    UserID = userId,
                    ProductID = productId,
                    Amount = amount
                });
            }
            else
            {
                basketPosition.Amount += amount;
            }

            _context.SaveChanges();
        }

        public void UpdateBasketPosition(int userId, int productId, int newAmount)
        {
            var basketPosition = _context.BasketPositions
                .FirstOrDefault(bp => bp.UserID == userId && bp.ProductID == productId);

            if (basketPosition != null)
            {
                basketPosition.Amount = newAmount;
                _context.SaveChanges();
            }
        }

        public void RemoveFromBasket(int userId, int productId)
        {
            var basketPosition = _context.BasketPositions
                .FirstOrDefault(bp => bp.UserID == userId && bp.ProductID == productId);

            if (basketPosition != null)
            {
                _context.BasketPositions.Remove(basketPosition);
                _context.SaveChanges();
            }
        }
    }

}
