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
    public class ProductStatusesController : ControllerBase
    {
        private readonly WebRestOracleContext _context;

        public ProductStatusesController(WebRestOracleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStatus>>> Get()
        {
            return await _context.ProductStatuses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductStatus>> Get(int id)
        {
            var productStatus = await _context.ProductStatuses.FindAsync(id);
            if (productStatus == null) return NotFound();
            return productStatus;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductStatus productStatus)
        {
            if (id != productStatus.ProductStatusId) return BadRequest();
            _context.ProductStatuses.Update(productStatus);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!ProductStatusExists(id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductStatus>> Post(ProductStatus productStatus)
        {
            _context.ProductStatuses.Add(productStatus);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = productStatus.ProductStatusId }, productStatus);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productStatus = await _context.ProductStatuses.FindAsync(id);
            if (productStatus == null) return NotFound();
            _context.ProductStatuses.Remove(productStatus);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductStatusExists(int id)
        {
            return _context.ProductStatuses.Any(e => e.ProductStatusId == id);
        }
    }

}