﻿using System.ComponentModel.DataAnnotations;

namespace NZ_Walk.Models.DTO
{
    public class LoginRequestDtos
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
