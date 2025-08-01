﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class AuthApiDto
    {
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public required string Password { get; set; }

    }
}
