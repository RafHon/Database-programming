using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IProductGroupService
    {
        IEnumerable<ProductGroupResponseDTO> GetProductGroups(
            int? parentId = null,
            string? sortBy = "Name",
            bool ascending = true
        );

        void AddProductGroup(string name, int? parentId);
    }
}
