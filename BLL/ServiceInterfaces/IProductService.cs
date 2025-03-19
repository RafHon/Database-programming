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
        IEnumerable<ProductResponseDTO> GetProducts(string sortBy, bool ascending, string filterName, string filterGroupName, int filterGroupID, bool includeInactive);
        void AddProduct(ProductRequestDTO product);
        void DeactivateProduct(int productID);
        void ActivateProduct(int productID);
        void DeleteProduct(int productID);
        
    }
}
