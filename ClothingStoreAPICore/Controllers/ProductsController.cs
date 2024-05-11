using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClothingStoreAPICore.Model;
using Microsoft.Extensions.Hosting;

namespace ClothingStoreAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ClothingStoreContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(ClothingStoreContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("order/{idOrder}")]
        public async Task<ActionResult<Product>> GetProductListByOrder(int idOrder)
        {
            var orderDetail = await _context.OrderDetails.Where(order => order.OrderId == idOrder).ToListAsync();
            var listProd = new List<Product>();
            foreach (var item in orderDetail)
            {
                Product prod = await _context.Products.FindAsync(item.ProductId);
                listProd.Add(prod);
            }

            return Ok(listProd);
        }
      

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] Product product)
        {
            var existingProd = await _context.Products.FirstOrDefaultAsync(s => s.ProductId == id);
            if (existingProd == null)
            {
                return NotFound();
            }

            existingProd.CategoryId = product.CategoryId;
            existingProd.ProductName = product.ProductName;
            existingProd.CreatedDate = product.CreatedDate;
            existingProd.InitialPrice = product.InitialPrice;
            existingProd.OfficialPrice = product.OfficialPrice;
            existingProd.Amount1 = product.Amount1;
            existingProd.Amount2 = product.Amount2;
            existingProd.Amount3 = product.Amount3;
            existingProd.Size1 = product.Size1;
            existingProd.Size2 = product.Size2;
            existingProd.Size3 = product.Size3;
            existingProd.Image1 = product.Image1;
            existingProd.Image2 = product.Image2;
            existingProd.Image3 = product.Image3;
            existingProd.ImgPath1 = await UploadImageAsync(product.Image1);
            existingProd.ImgPath2 = await UploadImageAsync(product.Image2);
            existingProd.ImgPath3 = await UploadImageAsync(product.Image3);
            existingProd.Introduction = product.Introduction;

            await _context.SaveChangesAsync();

            return Ok(existingProd);
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Product>> PostProduct(Product product)
        //{
        //    if (_context.Products == null)
        //    {
        //        return Problem("Entity set 'ClothingStoreContext.Products'  is null.");
        //    }
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        //}
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] Product prod)
        {
            try
            {
                if (prod.Image1 != null)
                {
                    prod.ImgPath1 = await UploadImageAsync(prod.Image1);
                }

                if (prod.Image2 != null)
                {
                    prod.ImgPath2 = await UploadImageAsync(prod.Image2);
                }

                if (prod.Image3 != null)
                {
                    prod.ImgPath3 = await UploadImageAsync(prod.Image3);
                }

                DateTime now = DateTime.UtcNow;
                prod.CreatedDate = now;

                _context.Products.Add(prod);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduct", new { id = prod.ProductId }, prod);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("searchKey={key}")]
        public async Task<ActionResult<Product>> SearchProdByKeyString(string key)
        {
            if (key != null)
            {
                var listProd = await _context.Products.Where(prod => prod.ProductName.Contains(key)).ToListAsync();
                if (listProd == null)
                {
                    return NotFound();
                }
                return Ok(listProd);
            }
            return BadRequest("Product Not Found");
        }
        [HttpGet("idFavor={idFavor}")]
        public async Task<ActionResult<Product>> GetFavoriteProductList(int idFavor)
        {
            var favorProd = await _context.FavoriteProducts.Where(favor => favor.FavoriteId == idFavor).ToListAsync();
            var lstProd = await _context.FavoriteProducts.Where(fprod => fprod.UserId == idFavor).ToListAsync();
            foreach (var item in favorProd)
            {
                Product prod = await _context.Products.FindAsync(item.ProductId);
            }
            return Ok(lstProd);
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
        private async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            string uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string relativeImagePath = Path.Combine("uploads", uniqueFileName);
            string imagePath = Path.Combine(_environment.WebRootPath, relativeImagePath);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Thay đổi đường dẫn về định dạng chéo (/)
            relativeImagePath = relativeImagePath.Replace("\\", "/");

            return relativeImagePath;
        }
        [HttpPut]
        [Route("api/products/{productId}/reduceQuantity")]
        public async Task<IActionResult> ReduceProductQuantity(int productId, string size, int amountToReduce)
        {
            // Lấy thông tin sản phẩm từ cơ sở dữ liệu bằng productId
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if(size == "S")
            {
                product.Amount1 -= amountToReduce;
            }else if(size == "M")
            {
                product.Amount2 -= amountToReduce;
            }else if(size == "L")
            {
                product.Amount3 -= amountToReduce;
            }
            
            _context.SaveChanges();

            return Ok("Số lượng sản phẩm đã được giảm.");
        }
        [HttpPut]
        [Route("api/products/{productId}/increaseQuantity")]
        public async Task<IActionResult> IncreaseProductQuantity(int productId, string size, int amountToIncrease)
        {
            // Lấy thông tin sản phẩm từ cơ sở dữ liệu bằng productId
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (size == "S")
            {
                product.Amount1 += amountToIncrease;
            }
            else if (size == "M")
            {
                product.Amount2 += amountToIncrease;
            }
            else if (size == "L")
            {
                product.Amount3 += amountToIncrease;
            }

            _context.SaveChanges();

            return Ok("Số lượng sản phẩm đã được tăng lại.");
        }
        [HttpGet("sort")]
        public IActionResult GetProducts(string sortOrder)
        {
            var products = _context.Products.AsQueryable();
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.OfficialPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.OfficialPrice);
                    break;
                default:
                    break;
            }
            return Ok(products.ToList());
        }
        [HttpGet("filter_input")]
        public IActionResult GetProductsByPriceRange(int minPrice, int maxPrice)
        {
            var products = _context.Products.Where(p => p.OfficialPrice >= minPrice && p.OfficialPrice <= maxPrice).ToList();
            return Ok(products);
        }

    }
}
