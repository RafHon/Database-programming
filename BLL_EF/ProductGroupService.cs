using BLL.DTOModels;
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
    public class ProductGroupService : IProductGroupService
    {
        private readonly WebStoreContext _context;

        public ProductGroupService(WebStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductGroupResponseDTO> GetProductGroups(
            int? parentId = null,
            string sortBy = "Name",
            bool ascending = true
        )
        {
            var query = _context.ProductGroups.AsQueryable();

            if (parentId.HasValue)
            {
                query = query.Where(pg => pg.ParentID == parentId);
            }

           
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                query = ascending ? query.OrderBy(pg => pg.Name) : query.OrderByDescending(pg => pg.Name);
            }
            else if (sortBy.Equals("ID", StringComparison.OrdinalIgnoreCase))
            {
                query = ascending ? query.OrderBy(pg => pg.ID) : query.OrderByDescending(pg => pg.ID);
            }

            return query.Select(pg => new ProductGroupResponseDTO(
                pg.ID,                       
                pg.Name,                     
                pg.ChildGroups.Any()         
            )).ToList();
        }

        public void AddProductGroup(string name, int? parentId)
        {
            var productGroup = new ProductGroup
            {
                Name = name,
                ParentID = parentId
            };

            _context.ProductGroups.Add(productGroup);
            _context.SaveChanges();
        }
    }

}
