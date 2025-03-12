using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IBasketService
    {
        void AddToBasket(int userId, int productId, int amount);

        void UpdateBasketPosition(int userId, int productId, int newAmount);

        void RemoveFromBasket(int userId, int productId);
    }
}
