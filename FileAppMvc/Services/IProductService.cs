namespace FileAppMvc.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();
        Task AddProduct(Product product,IFormFile file);
        Task UpdateProduct(Product product, IFormFile file);
        Task<Product> FindProduct(int id);
        Task DeleteProduct(int id);
    }
}
