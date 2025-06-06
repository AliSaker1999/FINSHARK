using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MaxLength(10,ErrorMessage ="Symbol cannot be over 10 charachters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(10,ErrorMessage ="Company name cannot be over 10 charachters")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1,1000000000)]
        public decimal MyProperty {get; set;}
        
        [Required]
        [Range(0.001,100)]
        public decimal LastDiv {get; set;}
        [Required]
        [MaxLength(10,ErrorMessage ="Industry name cannot be over 10 charachters")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1,10000000000)]
        public long MarketCap { get; set; }
    }
}