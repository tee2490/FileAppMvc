using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileAppMvc.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext dataContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductService(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dataContext = dataContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await dataContext.Products.ToListAsync();
        }

        public async Task AddProduct(Product product, IFormFile file)
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(file.FileName);
                var uploads = Path.Combine(wwwRootPath, "images");

                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                //แบบที่ 1 บันทึกรุปภาพใหม่(เป็นไฟล์ภายนอก database)
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                product.Image = @"\images\" + fileName + extension;

                //แบบที่ 2 บันทึกเนื้อไฟล์แบบ Base64 (ภายใน database)
                using (var memoryStream = new MemoryStream())
                {
                    var format = "image/png";
                    await file.CopyToAsync(memoryStream);
                    product.ImageBase64 = $"data:{format};base64,{Convert.ToBase64String(memoryStream.ToArray())}";
                }

            }

            await dataContext.Products.AddAsync(product);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Product> FindProduct(int id)
        {
            return await dataContext.Products.FindAsync(id);
        }

        public async Task UpdateProduct(Product product, IFormFile file)
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(file.FileName);
                var uploads = Path.Combine(wwwRootPath, "images");

                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                //กรณีมีรูปภาพเดิมต้องลบทิ้งก่อน
                if (product.Image != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, product.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                //แบบที่ 1 บันทึกรุปภาพใหม่(เป็นไฟล์ภายนอก database)
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                product.Image = @"\images\" + fileName + extension;

                //แบบที่ 2 บันทึกเนื้อไฟล์แบบ Base64 (ภายใน database)
                using (var memoryStream = new MemoryStream())
                {
                    var format = "image/png";
                    await file.CopyToAsync(memoryStream);
                    product.ImageBase64 = $"data:{format};base64,{Convert.ToBase64String(memoryStream.ToArray())}";
                }
            }

            dataContext.Products.Update(product);
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await FindProduct(id);

            var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, product.Image.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            dataContext.Products.Remove(product);
            await dataContext.SaveChangesAsync();

        }
    }
}
