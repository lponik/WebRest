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
public class AddressesController : ControllerBase
{
    private readonly WebRestOracleContext _context;

    public AddressesController(WebRestOracleContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Address>>> Get()
    {
        return await _context.Addresses.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> Get(int id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null) return NotFound();
        return address;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Address address)
    {
        if (id != address.AddressId) return BadRequest();
        _context.Addresses.Update(address);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) { if (!AddressExists(id)) return NotFound(); else throw; }
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Address>> Post(Address address)
    {
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = address.AddressId }, address);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null) return NotFound();
        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool AddressExists(int id)
    {
        return _context.Addresses.Any(e => e.AddressId == id);
    }
}
}
