using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IBasketService
    {
        void AddToBasket(int userID, int productID, int amount);
        void UpdateBasketItem(int userID, int productID, int amount);
        void RemoveFromBasket(int userID, int productID);
        int GenerateOrder(int userID);
    }
}
