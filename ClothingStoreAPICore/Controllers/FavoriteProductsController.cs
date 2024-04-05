using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClothingStoreAPICore.Model;

namespace ClothingStoreAPICore.Controllers
{
    // FavoriteProductController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteProductController : ControllerBase
    {
        private readonly ClothingStoreContext _context;

        public FavoriteProductController(ClothingStoreContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FavoriteProduct>>> GetFavoritesByUserId(int userId)
        {
            var favorites = await _context.FavoriteProducts.Where(fp => fp.UserId == userId).ToListAsync();

            return Ok(favorites);
        }

        [HttpPost]
        public async Task<ActionResult<FavoriteProduct>> AddToFavorites([FromBody] FavoriteProduct favoriteProduct)
        {
            // Kiểm tra xem sản phẩm đã tồn tại trong danh sách yêu thích của người dùng chưa
            var existingFavorite = await _context.FavoriteProducts.FirstOrDefaultAsync(fp => fp.UserId == favoriteProduct.UserId && fp.ProductId == favoriteProduct.ProductId);

            if (existingFavorite != null)
            {
                return BadRequest("Sản phẩm đã tồn tại trong danh sách yêu thích của người dùng.");
            }

            _context.FavoriteProducts.Add(favoriteProduct);
            await _context.SaveChangesAsync();
            return Ok(favoriteProduct);
        }

        [HttpDelete("{FavoriteId}")]
        public async Task<ActionResult> RemoveFromFavorites(int FavoriteId)
        {
            var favoriteProduct = await _context.FavoriteProducts.FindAsync(FavoriteId);
            if (favoriteProduct == null)
            {
                return NotFound();
            }

            _context.FavoriteProducts.Remove(favoriteProduct);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
