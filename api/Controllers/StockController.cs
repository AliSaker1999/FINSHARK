using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Repository;
using api.Interfaces;
using api.Mappers;
using api.Dtos.Stock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context=context;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks= await _stockRepo.GetAllAsync();
            var stockDto =stocks.Select(s=>s.ToStockDto());
            return Ok(stockDto);
        }



        [HttpGet("{id}")]
        public async Task <IActionResult> GetById([FromRoute] int id)
        {
            var stocks= await _stockRepo.GetByIdAsync(id);
            if(stocks==null){
                return NotFound();
            }
            return Ok(stocks.ToStockDto());
        }

        [HttpPost]
        public async Task <IActionResult> Create([FromBody]CreateStockRequestDto stockDto)
        {
            var stocks=stockDto.ToStockFromCreateDTO();
            await _context.Stock.AddAsync(stocks);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new{id=stocks.Id},stocks.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task <IActionResult> Update([FromRoute] int id, [FromBody]UpdateStockRequestDto updateDto)
        {   
            var stockModel= await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);

            if(stockModel == null)
            {
                return NotFound();
            }
            
            stockModel.Symbol=updateDto.Symbol;
            stockModel.CompanyName=updateDto.CompanyName;
            stockModel.MyProperty=updateDto.MyProperty;
            stockModel.LastDiv=updateDto.LastDiv;
            stockModel.Industry=updateDto.Industry;
            stockModel.MarketCap=updateDto.MarketCap;
            await _context.SaveChangesAsync();
            
            
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task <IActionResult> Delete([FromRoute] int id)
        {   
            var stockModel=await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);

            if(stockModel == null)
            {
                return NotFound();
            }
            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}