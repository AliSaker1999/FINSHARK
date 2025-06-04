using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
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

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stock.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.isDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }
            var skipNum = (query.PageNumber - 1) * query.PageSize;


            return await stocks.Skip(skipNum).Take(query.PageSize).ToListAsync();

        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.Include(c=>c.Comments).ThenInclude(a=>a.AppUser).FirstOrDefaultAsync(i=>i.Id==id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
            
        }

        public async Task<bool> stockExist(int id)
        {
            return await _context.Stock.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> updateAsync(int id, UpdateStockRequestDto stockDto)
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