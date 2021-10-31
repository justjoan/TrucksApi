using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace TrucksApi.Models
{
    [Index(nameof(Block))]
    public class Truck
    {
        [Required]
        public string Id { get; set; }    
        public string Source { get; set; } 
        public string Region { get; set; } 
        public string Block { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
