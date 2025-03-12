using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IProductService
    {
        IEnumerable<ProductResponseDTO> GetProducts(
            string? name = null,
            string? groupName = null,
            int? groupId = null,
            bool onlyActive = true,
            string sortBy = "Name",
            bool ascending = true
        );

        void AddProduct(string name, double price, int groupId);

        void DeactivateOrDeleteProduct(int productId, bool delete = false);

        void ActivateProduct(int productId);
    }
}
