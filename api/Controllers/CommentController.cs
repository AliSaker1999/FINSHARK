using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Repository;
using api.Interfaces;
using api.Mappers;
using api.Dtos.Comment;
using Microsoft.AspNetCore.Identity;
using api.Models;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _UserManager;
        public CommentController(ICommentRepository commentrepo, IStockRepository stockrepo, UserManager<AppUser> UserManager)
        {
            _commentRepo = commentrepo;
            _stockRepo = stockrepo;
            _UserManager = UserManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepo.GetAllAsync();
            var commentDto = comment.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpPost("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentkDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (!await _stockRepo.stockExist(stockId))
            {
                return BadRequest("stock does not exist");
            }
            var username = User.GetUsername();
            var AppUser = await _UserManager.FindByNameAsync(username);

            var commentModel = commentkDto.ToCreateCommentDto(stockId);
            commentModel.AppUserId = AppUser.Id;
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());

        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateCommentRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepo.updateAsync(id, updateDto.ToCommentFromUpdate());
            if (comment == null)
            {
                return NotFound("comment not found");
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task <IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var commentModel = await _commentRepo.DeleteAsync(id);

            if(commentModel == null)
            {
                return NotFound("comment does not exist");
            }
            return NoContent();

        }
    }
}



