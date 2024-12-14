using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var stocks = _context.Stocks.ToList()
                .Select(stock => stock.ToDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToModelFromDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            // get the ID of the newly created stock 
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
        {
            var stock = _context.Stocks.Find(id);
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
            _context.SaveChanges();
            return Ok(stock.ToDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            _context.Stocks.Remove(stock);
            _context.SaveChanges();
            return NoContent();
        }
    }
}