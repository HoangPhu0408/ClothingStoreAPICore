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
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ClothingStoreContext _context;

        public CustomersController(ClothingStoreContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.UserId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPost("signin")]
        public async Task<ActionResult<Customer>> LoginAccount([FromBody] Customer cus)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(s => s.PhoneNumber == cus.PhoneNumber);
            var admin = await _context.AdminAccounts.FirstOrDefaultAsync(s => s.AdminUserName == cus.PhoneNumber && s.AdminPassword == cus.Password);
            if (admin != null)
                return Ok(new { message = "Admin Status Success!" });
            if (customer == null)
                return Unauthorized(new { message = "Tai khoan khong ton tai" });
            if (cus.Password != customer.Password)
            {
                return Unauthorized(new { message = "Mat khau khong dung" });
            }
            return Ok(customer);
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
          if (_context.Customers == null)
          {
              return Problem("Entity set 'ClothingStoreContext.Customers'  is null.");
          }
          else if (ClientExist(customer.PhoneNumber))
          {
              return Problem("Tài khoản đã tồn tại");
          }
          _context.Customers.Add(customer);
          await _context.SaveChangesAsync();
          return CreatedAtAction("GetCustomer", new { id = customer.UserId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ClientExist(string phone)
        {
            return (_context.Customers?.Any(e => e.PhoneNumber == phone)).GetValueOrDefault();
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

    }
}
