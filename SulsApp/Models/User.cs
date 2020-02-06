namespace SulsApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }


        [MaxLength(20)]
        [Required]
        public string Username { get; set; }


        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


    }
}
