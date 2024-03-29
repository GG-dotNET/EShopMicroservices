﻿using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products.Find(x => true).ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _context.Products.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Name, name);
        
        return await _context.Products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Category, categoryName);
        
        return await _context.Products.Find(filter).ToListAsync();
    }

    public async Task CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _context.Products.ReplaceOneAsync(x => x.Id.Equals(product.Id), product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
        var deleteResult =  await _context.Products.DeleteOneAsync(filter);
        
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}