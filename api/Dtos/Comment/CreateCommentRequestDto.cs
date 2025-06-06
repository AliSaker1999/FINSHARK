using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Mappers;
namespace api.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 charachters")]
        [MaxLength(280,ErrorMessage ="Title cannot be over 280 charachters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Content must be 5 charachters")]
        [MaxLength(280,ErrorMessage ="Content cannot be over 280 charachters")]
        public string Content { get; set; } = string.Empty;
        
    }
}