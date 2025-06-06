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
using api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        
        private readonly IStockRepository _stockRepo;
        
        public StockController( IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto()).ToList();
            return Ok(stockDto);
        }



        [HttpGet("{id:int}")]
        public async Task <IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks= await _stockRepo.GetByIdAsync(id);
            if(stocks==null){
                return NotFound();
            }
            return Ok(stocks.ToStockDto());
        }

        [HttpPost]
        public async Task <IActionResult> Create([FromBody]CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks=stockDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stocks);
            return CreatedAtAction(nameof(GetById), new{id=stocks.Id},stocks.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task <IActionResult> Update([FromRoute] int id, [FromBody]UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await _stockRepo.updateAsync(id, updateDto);

            if(stockModel == null)
            {
                return NotFound();
            }
            
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task <IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await _stockRepo.DeleteAsync(id);

            if(stockModel == null)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}