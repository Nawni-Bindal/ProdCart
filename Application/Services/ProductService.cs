using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            // Retrieve all Product entities from the repository  
            var productEntities = await _productRepository.GetAllProductsAsync();
            if (productEntities == null || !productEntities.Any())
            {
                return Enumerable.Empty<ProductDto>();
            }
            // Map the Product entities to a list of ProductDto  
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            // Retrieve the Product entity by ID from the repository  
            var productEntity = await _productRepository.GetProductByIdAsync(id);            

            // Map the Product entity to a ProductDto  
            return  _mapper.Map<ProductDto>(productEntity);             
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            // Map the CreateProductDto to a Product entity  
            var productEntity = _mapper.Map<Product>(createProductDto);
            productEntity.CreatedOn = DateTime.UtcNow;

            // Add the Product entity to the database  
            await _productRepository.CreateProductAsync(productEntity);            

            // Map the saved Product entity back to a ProductDto  
            var productDto = _mapper.Map<ProductDto>(productEntity);
            return productDto;
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            // Retrieve the existing Product entity by ID  
            var existingProductEntity = await _productRepository.GetProductByIdAsync(id);
            if (existingProductEntity == null)
            {
                return null; // Product not found
            }
            // Map the UpdateProductDto to the existing Product entity  
            _mapper.Map(updateProductDto, existingProductEntity);
            existingProductEntity.ModifiedOn = DateTime.UtcNow;
            // Update the Product entity in the database  
            await _productRepository.UpdateProductAsync(id,existingProductEntity);
            // Map the updated Product entity back to a ProductDto  
            var updatedProductDto = _mapper.Map<ProductDto>(existingProductEntity);
            return updatedProductDto;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            // Attempt to delete the product by its ID  
            var success = await _productRepository.DeleteProductAsync(id);
            return success;
        }        
    }
}