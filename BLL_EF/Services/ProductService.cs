using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ServiceInterfaces;
using BLL.DTOModels;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace BLL_EF.Services
{
    public class ProductService : IProductService
    {
        private readonly WebstoreContext _context;

        public ProductService(WebstoreContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductResponseDTO> GetProducts(string sortBy, bool ascending, string filterName, string filterGroupName, int filterGroupID, bool includeInactive)
        {
            var query = _context.Products
                .Include(p => p.ProductGroup)
                .ThenInclude(pg => pg.ParentGroup)
                .AsQueryable();

            if (!includeInactive)
                query = query.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(filterName))
                query = query.Where(p => p.Name.Contains(filterName));

            if (filterGroupID > 0)
                query = query.Where(p => p.GroupID == filterGroupID);

            query = sortBy switch
            {
                "name" => ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
                "price" => ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                "group" => ascending ? query.OrderBy(p => p.ProductGroup.Name) : query.OrderByDescending(p => p.ProductGroup.Name),
                _ => query
            };

            var products = query.ToList();

            if (!string.IsNullOrEmpty(filterGroupName))
            {
                products = products.Where(p =>
                {
                    var group = p.ProductGroup;
                    while (group != null)
                    {
                        if (group.Name.Contains(filterGroupName))
                            return true;
                        group = _context.ProductGroups.FirstOrDefault(g => g.ID == group.ParentID);
                    }
                    return false;
                }).ToList();
            }

            var result = products.Select(p => new ProductResponseDTO(
                p.ID,
                p.Name,
                p.Price,
                p.Image,
                p.IsActive,
                BuildGroupPath(p.ProductGroup)
            )).ToList();

            return result;
        }



        public void AddProduct(ProductRequestDTO product)
        {
            var newProduct = new Product { Name = product.Name, Price = product.Price, Image = product.Image, GroupID = product.GroupID, IsActive = true };
            _context.Products.Add(newProduct);
            _context.SaveChanges();
        }

        private string BuildGroupPath(ProductGroup group)
        {
            var pathParts = new List<string>();
            
            while (group != null)
            {
                pathParts.Insert(0, group.Name);
                _context.ProductGroups.Find(group.ParentID);
                group = group.ParentGroup;
            }

            return string.Join("/", pathParts);
        }

        public void DeactivateProduct(int productID)
        {
            var product = _context.Products.Find(productID);
            if (product != null)
            {
                product.IsActive = false;
                _context.SaveChanges();
            }
        }

        public void ActivateProduct(int productID)
        {
            var product = _context.Products.Find(productID);
            if (product != null)
            {
                product.IsActive = true;
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int productID)
        {
            var product = _context.Products.Find(productID);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
