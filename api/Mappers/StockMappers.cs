using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Dtos.Stock;


namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto  ToStockDto (this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                MyProperty = stockModel.MyProperty,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments=stockModel.Comments.Select(c=>c.ToCommentDto()).ToList()
            };
        }

        public static Stock  ToStockFromCreateDTO (this CreateStockRequestDto CreateStockRequestDto)
        {
            return new Stock
            {
                Symbol = CreateStockRequestDto.Symbol,
                CompanyName=CreateStockRequestDto.CompanyName,
                MyProperty=CreateStockRequestDto.MyProperty,
                LastDiv=CreateStockRequestDto.LastDiv,
                Industry=CreateStockRequestDto.Industry,
                MarketCap=CreateStockRequestDto.MarketCap
            };
        }

        
    }
}