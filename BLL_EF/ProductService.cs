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
    public class ProductService : IProductService
    {
        private readonly WebStoreContext _context;

        public ProductService(WebStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductResponseDTO> GetProducts(
        string? name = null,
        string? groupName = null,
        int? groupId = null,
        bool onlyActive = true,
        string sortBy = "Name",
        bool ascending = true
    )
        {
            var query = _context.Products.AsQueryable();

            if (onlyActive)
            {
                query = query.Where(p => p.IsActive);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (groupId.HasValue)
            {
                query = query.Where(p => p.GroupID == groupId);
            }

            if (!string.IsNullOrEmpty(groupName))
            {
                query = query.Include(p => p.Group).Where(p => p.Group.Name.Contains(groupName));
            }

            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                query = ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
            }
            else if (sortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
            {
                query = ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
            }
            query = ascending
            ? query.OrderBy(p => p.Group != null ? p.Group.Name : string.Empty)
            : query.OrderByDescending(p => p.Group != null ? p.Group.Name : string.Empty);

            return query.Select(p => new ProductResponseDTO(
                p.ID,
                p.Name,
                p.Price,
                GetFullGroupName(p.Group.ID)
            )).ToList();
        }

        private string GetFullGroupName(int groupId)
        {
            var group = _context.ProductGroups.Include(g => g.ParentGroup)
                                              .FirstOrDefault(g => g.ID == groupId);

            if (group == null)
                throw new Exception("Group not found");

            var fullGroupName = group.Name;

            if (group.ParentGroup != null)
            {
                fullGroupName = GetFullGroupName(group.ParentGroup.ID) + " / " + fullGroupName;
            }

            return fullGroupName;
        }

        public void AddProduct(string name, double price, int groupId)
        {
            var product = new Product
            {
                Name = name,
                Price = price,
                GroupID = groupId,
                IsActive = true
            };

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeactivateOrDeleteProduct(int productId, bool delete = false)
        {
            var product = _context.Products.FirstOrDefault(p => p.ID == productId);

            if (product == null)
                throw new Exception("Product not found");

            if (delete)
            {
                _context.Products.Remove(product);
            }
            else
            {
                product.IsActive = false;
            }

            _context.SaveChanges();
        }

        public void ActivateProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ID == productId);

            if (product == null)
                throw new Exception("Product not found");

            product.IsActive = true;
            _context.SaveChanges();
        }
    }
}
