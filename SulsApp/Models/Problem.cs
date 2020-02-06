using System.ComponentModel.DataAnnotations;

namespace SulsApp.Models
{
    public class Problem
    {
        public string Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        public int Points { get; set; }
    }
}
