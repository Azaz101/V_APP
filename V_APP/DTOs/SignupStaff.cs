﻿namespace V_APP.DTOs
{
    public class SignupStaff
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public int? SystemUserId { get; set; }
        public string? Gender { get; set; }
		public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
		public int? Type { get; set; }
	}
}