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
    public class OrdersLinesController : ControllerBase
    {
        private readonly WebRestOracleContext _context;

        public OrdersLinesController(WebRestOracleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderLine>>> Get()
        {
            return await _context.OrdersLines.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderLine>> Get(int id)
        {
            var orderLine = await _context.OrdersLines.FindAsync(id);
            if (orderLine == null) return NotFound();
            return orderLine;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, OrderLine orderLine)
        {
            if (id != orderLine.OrderLineId) return BadRequest();
            _context.OrdersLines.Update(orderLine);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!OrderLineExists(id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderLine>> Post(OrderLine orderLine)
        {
            _context.OrdersLines.Add(orderLine);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = orderLine.OrderLineId }, orderLine);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderLine = await _context.OrdersLines.FindAsync(id);
            if (orderLine == null) return NotFound();
            _context.OrdersLines.Remove(orderLine);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrderLineExists(int id)
        {
            return _context.OrdersLines.Any(e => e.OrderLineId == id);
        }
    }

}