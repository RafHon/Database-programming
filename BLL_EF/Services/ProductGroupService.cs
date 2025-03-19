using BLL.DTOModels;
using BLL.ServiceInterfaces;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF.Services
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly WebstoreContext _context;

        public ProductGroupService(WebstoreContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductGroupResponseDTO> GetProductGroups(int? parentID, bool ascending)
        {
            var query = _context.ProductGroups.AsQueryable();

            if (parentID.HasValue)
                query = query.Where(pg => pg.ParentID == parentID);

            query = ascending ? query.OrderBy(pg => pg.Name) : query.OrderByDescending(pg => pg.Name);

            return query.Select(pg => new ProductGroupResponseDTO(pg.ID, pg.Name, _context.ProductGroups.Any(g => g.ParentID == pg.ID))).ToList();
        }


        public void AddProductGroup(ProductGroupRequestDTO group)
        {
            var newGroup = new ProductGroup { Name = group.Name, ParentID = group.ParentID };
            _context.ProductGroups.Add(newGroup);
            _context.SaveChanges();
        }
    }
}
