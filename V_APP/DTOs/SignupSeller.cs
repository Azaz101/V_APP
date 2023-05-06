namespace V_APP.DTOs
{
    public class SignupSeller
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
		public string? Gender { get; set; }
		public string? Address { get; set; }
        public int? Cnic { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
		public string? CompanyName { get; set; }
		public string? City { get; set; }
		public string? ShortDescription { get; set; }
		public string? LongDescription { get; set; }
		public string? PhoneNumber { get; set; }
		public DateTime? Dob { get; set; }
	}
}
