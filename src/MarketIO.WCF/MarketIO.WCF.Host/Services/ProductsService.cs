using MarketIO.WCF.Contracts;
using System.Collections.Generic;

namespace MarketIO.WCF.Host.Services
{
    public class ProductsService : IProductsService
    {
        private static List<Product> _repository = new List<Product>()
        {
            new Product
            {
                Id =1,
                Name = "TShirt",
                Price = 50
            },
            new Product
            {
                Id=2,
                Name= "Skirt",
                Price = 213
            }
        };

        public Product AddProduct(int id , string name, double price)
        {
            var product = new Product
            {
                Id = id,
                Name = name,
                Price = price
            };
            _repository.Add(product);
            return product;
        }

        public List<Product> GetProducts() => _repository;
    }
}
