using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ProdCart.Infrastructure.Data;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            // Eagerly load items to avoid N+1 queries if items are needed
            return await _context.Products
                .Include(p => p.Items)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            // Eagerly load items for single product
            return await _context.Products
                .Include(p => p.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? new Product();
        }

        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            if (id != product.Id)
                return null;

            var existingProduct = await _context.Products
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
                return null;

            // Update scalar properties
            _context.Entry(existingProduct).CurrentValues.SetValues(product);

            // Efficiently update items collection
            if (product.Items != null)
            {
                // Remove items not present in the updated product
                var updatedItemIds = product.Items.Select(i => i.Id).ToHashSet();
                foreach (var existingItem in existingProduct.Items.ToList())
                {
                    if (!updatedItemIds.Contains(existingItem.Id))
                        _context.Items.Remove(existingItem);
                }

                // Add or update items
                foreach (var updatedItem in product.Items)
                {
                    var existingItem = existingProduct.Items.FirstOrDefault(i => i.Id == updatedItem.Id);
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(updatedItem);
                    }
                    else
                    {
                        // Ensure the new item is linked to the product
                        updatedItem.ProductId = existingProduct.Id;
                        existingProduct.Items.Add(updatedItem);
                    }
                }
            }

            existingProduct.ModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}