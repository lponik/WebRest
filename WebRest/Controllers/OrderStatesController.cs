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
    public class OrderStatesController : ControllerBase
    {
        private readonly WebRestOracleContext _context;

        public OrderStatesController(WebRestOracleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderState>>> Get()
        {
            return await _context.OrderStates.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderState>> Get(int id)
        {
            var orderState = await _context.OrderStates.FindAsync(id);
            if (orderState == null) return NotFound();
            return orderState;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, OrderState orderState)
        {
            if (id != orderState.OrderStateId) return BadRequest();
            _context.OrderStates.Update(orderState);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!OrderStateExists(id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderState>> Post(OrderState orderState)
        {
            _context.OrderStates.Add(orderState);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = orderState.OrderStateId }, orderState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderState = await _context.OrderStates.FindAsync(id);
            if (orderState == null) return NotFound();
            _context.OrderStates.Remove(orderState);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrderStateExists(int id)
        {
            return _context.OrderStates.Any(e => e.OrderStateId == id);
        }
    }

}