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
public class CustomerAddressesController : ControllerBase
{
    private readonly WebRestOracleContext _context;

    public CustomerAddressesController(WebRestOracleContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerAddress>>> Get()
    {
        return await _context.CustomerAddresses.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerAddress>> Get(int id)
    {
        var customerAddress = await _context.CustomerAddresses.FindAsync(id);
        if (customerAddress == null) return NotFound();
        return customerAddress;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CustomerAddress customerAddress)
    {
        if (id != customerAddress.CustomerAddressId) return BadRequest();
        _context.CustomerAddresses.Update(customerAddress);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) { if (!CustomerAddressExists(id)) return NotFound(); else throw; }
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<CustomerAddress>> Post(CustomerAddress customerAddress)
    {
        _context.CustomerAddresses.Add(customerAddress);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = customerAddress.CustomerAddressId }, customerAddress);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customerAddress = await _context.CustomerAddresses.FindAsync(id);
        if (customerAddress == null) return NotFound();
        _context.CustomerAddresses.Remove(customerAddress);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool CustomerAddressExists(int id)
    {
        return _context.CustomerAddresses.Any(e => e.CustomerAddressId == id);
    }
}

}