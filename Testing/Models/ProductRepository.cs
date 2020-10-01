using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;

namespace Testing.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;
        public ProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public Product AssignCategory()
        {
            var categoryList = GetCategories();
            var product = new Product();
            product.Categories = categoryList;
            return product;
        }

        public void DeleteProduct(Product product)
        {
            _conn.Execute("DELETE FROM Reviews WHERE ProductID = @productID",
                new { productID = product.ProductID });
            _conn.Execute("DELETE FROM Sales WHERE ProductID = @productID",
                new { productID = product.ProductID });
            _conn.Execute("DELETE FROM Products WHERE ProductID = @productID",
                new { productID = product.ProductID });
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("SELECT * FROM Products;");
        }

        public IEnumerable<Category> GetCategories()
        {
            return _conn.Query<Category>("SELECT * FROM Categories;");
        }

        public Product GetProduct(int id)
        {
            return _conn.QuerySingle<Product>("SELECT * FROM Products WHERE ProductID = @productID;",
                new { productID = id });
        }

        public void InsertProduct(Product productToInsert)
        {
            _conn.Execute("INSERT INTO Products (Name, Price, CategoryID) VALUES (@Name, @Price, @CategoryID);",
                new { Name = productToInsert.Name, Price = productToInsert.Price, CategoryID = productToInsert.CategoryID });
        }

        public IEnumerable<Product> SearchProduct(string searchString)
        {
            return _conn.Query<Product>("SELECT * FROM Products WHERE Name LIKE @name;",
                new { name = "%" + searchString + "%" });
        }

        public void UpdateProduct(Product product)
        {
            _conn.Execute("UPDATE Products SET Name = @name, Price = @price WHERE ProductID = @id;",
                new { name = product.Name, price = product.Price, id = product.ProductID });
        }
    }
}
