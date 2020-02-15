using Andreys.Models;
using Andreys.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Andreys.Services
{
    public interface IProductsService
    {
        int Add(ProductAddInputModel productAddInputModel);

        // productDTO is like entity Product in this case
        IEnumerable<Product> GetAll();

        Product GetById(int id);

        void DeleteById(int id);
    }
}
