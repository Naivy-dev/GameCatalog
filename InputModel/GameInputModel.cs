using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.InputModel
{
    public class GameInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The game name must contain from 3 to 100 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(1500, MinimumLength = 10, ErrorMessage = "The game description must contain from 10 to 1500 characters")]
        public string Description { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The game developer name must contain from 3 to 100 characters")]
        public string Developer { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The game publisher name must contain from 3 to 100 characters")]
        public string Publisher { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "Minimum price of 1$ and maximum of 1000$")]
        public double Price { get; set; }
    }
}
