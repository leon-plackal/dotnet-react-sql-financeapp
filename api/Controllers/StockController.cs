using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stockDto = stocks.Select(stock => stock.ToDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToModelFromDto();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            // get the ID of the newly created stock 
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            stock.Symbol = updateDto.Symbol;
            stock.CompanyName = updateDto.CompanyName;
            stock.Purchase = updateDto.Purchase;
            stock.LastDiv = updateDto.LastDiv;
            stock.Industry = updateDto.Industry;
            stock.MarketCap = updateDto.MarketCap;
            await _context.SaveChangesAsync();
            return Ok(stock.ToDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}