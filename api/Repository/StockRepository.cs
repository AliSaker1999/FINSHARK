using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        
        public StockRepository(ApplicationDBContext context)
        {
            _context=context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockmodel=await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);
            if (stockmodel==null)
            {
                return null;
            }
            _context.Stock.Remove(stockmodel);
            await _context.SaveChangesAsync();
            return stockmodel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stock.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.FindAsync(id);
        }

        public async Task<Stock?> YpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stockModel =await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);
            if(stockModel==null)
            {
                return null;
            }
            stockModel.Symbol=stockDto.Symbol;
            stockModel.CompanyName=stockDto.CompanyName;
            stockModel.MyProperty=stockDto.MyProperty;
            stockModel.LastDiv=stockDto.LastDiv;
            stockModel.Industry=stockDto.Industry;
            stockModel.MarketCap=stockDto.MarketCap;
            await _context.SaveChangesAsync();
            return stockModel;

        }
    }
}