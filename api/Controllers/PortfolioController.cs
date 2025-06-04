using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly IStockRepository _StockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _UserManager = userManager;
            _StockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;

        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var AppUser = await _UserManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(AppUser);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var AppUser = await _UserManager.FindByNameAsync(username);
            var stock = await _StockRepo.GetBySymbolAsync(symbol);
            if (stock == null) return BadRequest("Stock Not Found");
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(AppUser);
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot Add Same Stock To Portfolio");
            var portfolio = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = AppUser.Id
            };
            var result = await _portfolioRepo.CreateAsync(portfolio);
            if (result == null) return StatusCode(500, "Could no Create");
            return Created();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var AppUser = await _UserManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(AppUser);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());
            if (filteredStock.Count() == 1)
            {
                await _portfolioRepo.DeleteAsync(AppUser, symbol);
            }
            else
            {
                return BadRequest("stock not in your portfolio");
            }
            return Ok();
        }

    }
}