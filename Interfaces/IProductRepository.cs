﻿using CodeKY_SD01.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeKY_SD01.Interfaces
{
    public interface IProductRepository
    {
        public string DbPath { get; }
        public void AddProduct(ProductEntity product);
        public void DeleteProduct(int id);
        public void UpdateProduct(ProductEntity product);
        // public void SaveChanges();
        public ProductEntity GetProductById(int id);
        public ProductEntity GetProductByName(string name);
        public IEnumerable<ProductEntity> GetAllProducts();
        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category);
        public IEnumerable<ProductEntity> GetAllProductsByName(string name);
        public IEnumerable<ProductEntity> GetOnlyInStockProducts();
    }
}
