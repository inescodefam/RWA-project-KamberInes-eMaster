﻿namespace WebAPI.DTOs
{
    public class UserApiDto
    {
        public int Iduser { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }
}