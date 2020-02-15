using Andreys.Services;
using Andreys.ViewModels.Products;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Andreys.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public HttpResponse Add()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(ProductAddInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            if (inputModel.Name.Length < 4 || inputModel.Name.Length > 20)
            {
                return this.View();
            }

            if (string.IsNullOrEmpty(inputModel.Description) || inputModel.Description.Length < 10)
            {
                return this.View();
            }

            var productId = this.productsService.Add(inputModel);

            return this.View();
        }

        public HttpResponse Details(int id)
        {
            var product = this.productsService.GetById(id);

            return this.View(product);
        }

        public HttpResponse Delete(int id)
        {
            this.productsService.DeleteById(id);

            return this.Redirect("/");
        }
    }
}
