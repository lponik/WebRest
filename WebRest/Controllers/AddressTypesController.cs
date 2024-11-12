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
    public class AddressTypesController : ControllerBase
    {
        private readonly WebRestOracleContext _context;

        public AddressTypesController(WebRestOracleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressType>>> Get()
        {
            return await _context.AddressTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressType>> Get(int id)
        {
            var addressType = await _context.AddressTypes.FindAsync(id);
            if (addressType == null) return NotFound();
            return addressType;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AddressType addressType)
        {
            if (id != addressType.AddressTypeId) return BadRequest();
            _context.AddressTypes.Update(addressType);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!AddressTypeExists(id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AddressType>> Post(AddressType addressType)
        {
            _context.AddressTypes.Add(addressType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = addressType.AddressTypeId }, addressType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var addressType = await _context.AddressTypes.FindAsync(id);
            if (addressType == null) return NotFound();
            _context.AddressTypes.Remove(addressType);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AddressTypeExists(int id)
        {
            return _context.AddressTypes.Any(e => e.AddressTypeId == id);
        }
    }
}
