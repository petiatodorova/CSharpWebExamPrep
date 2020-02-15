using Andreys.Data;
using Andreys.Models;
using Andreys.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andreys.Services
{
    public class ProductsService : IProductsService
    {
        private readonly AndreysDbContext dbContext;

        public ProductsService(AndreysDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Add(ProductAddInputModel productAddInputModel)
        {
            var genderAsEnum = Enum.Parse<Gender>(productAddInputModel.Gender);
            var categoryAsEnum = Enum.Parse<Category>(productAddInputModel.Category);

            var product = new Product()
            {
                Name = productAddInputModel.Name,
                Description = productAddInputModel.Description,
                ImageUrl = productAddInputModel.ImageUrl,
                Price = productAddInputModel.Price,
                Gender = genderAsEnum,
                Category = categoryAsEnum
            };

            this.dbContext.Products.Add(product);
            this.dbContext.SaveChanges();

            return product.Id;
        }

        public void DeleteById(int id)
        {
            var product = this.GetById(id);
            this.dbContext.Products.Remove(product);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            //first variant
            //return this.dbContext.Products.ToArray();

            //second variant
            return this.dbContext.Products.Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Price = x.Price
            }).ToArray();

        }

        public Product GetById(int id)
        {
            return this.dbContext.Products.FirstOrDefault(x => x.Id == id);
        }
    }
}
