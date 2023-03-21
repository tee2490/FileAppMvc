using FileAppMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileAppMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await productService.GetProducts());
        }

        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile file)
        {
            await productService.AddProduct(product, file);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await productService.FindProduct(id);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile file)
        {
            await productService.UpdateProduct(product, file);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteProduct(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
