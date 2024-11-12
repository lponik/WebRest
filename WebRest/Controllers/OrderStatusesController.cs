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
    public class OrderStatusesController : ControllerBase
    {
        private readonly WebRestOracleContext _context;

        public OrderStatusesController(WebRestOracleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderStatus>>> Get()
        {
            return await _context.OrderStatuses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderStatus>> Get(int id)
        {
            var orderStatus = await _context.OrderStatuses.FindAsync(id);
            if (orderStatus == null) return NotFound();
            return orderStatus;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, OrderStatus orderStatus)
        {
            if (id != orderStatus.OrderStatusId) return BadRequest();
            _context.OrderStatuses.Update(orderStatus);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!OrderStatusExists(id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderStatus>> Post(OrderStatus orderStatus)
        {
            _context.OrderStatuses.Add(orderStatus);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = orderStatus.OrderStatusId }, orderStatus);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderStatus = await _context.OrderStatuses.FindAsync(id);
            if (orderStatus == null) return NotFound();
            _context.OrderStatuses.Remove(orderStatus);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrderStatusExists(int id)
        {
            return _context.OrderStatuses.Any(e => e.OrderStatusId == id);
        }
}
}

