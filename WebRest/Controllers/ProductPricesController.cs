using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using WebRestEF.EF.Data;
using WebRestEF.EF.Models;
using WebRest.Interfaces;
namespace WebRest.Controllers
{
        [Route("api/[controller]")]
    [ApiController]
    public class ProductPricesController : ControllerBase
    {
        private readonly WebRestOracleContext _context;

        public ProductPricesController(WebRestOracleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductPrice>>> Get()
        {
            return await _context.ProductPrices.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductPrice>> Get(int id)
        {
            var productPrice = await _context.ProductPrices.FindAsync(id);
            if (productPrice == null) return NotFound();
            return productPrice;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductPrice productPrice)
        {
            if (id != productPrice.ProductPriceId) return BadRequest();
            _context.ProductPrices.Update(productPrice);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!ProductPriceExists(id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductPrice>> Post(ProductPrice productPrice)
        {
            _context.ProductPrices.Add(productPrice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = productPrice.ProductPriceId }, productPrice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productPrice = await _context.ProductPrices.FindAsync(id);
            if (productPrice == null) return NotFound();
            _context.ProductPrices.Remove(productPrice);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductPriceExists(int id)
        {
            return _context.ProductPrices.Any(e => e.ProductPriceId == id);
        }
    }

}