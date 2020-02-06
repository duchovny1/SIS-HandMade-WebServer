﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SulsApp.Models
{
    public class Submission
    {
        public string Id { get; set; }


        [Required]
        [MaxLength(800)]
        public string Code { get; set; }

        [Required]
        public int AchivedResults { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ProblemId { get; set; }
        public virtual Problem Problem { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }



    }
}
