namespace V_APP.DTOs
{
    public class SignupModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
		public string? Gender { get; set; }
		public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public int? Cnic { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
		public int? Type { get; set; }
	}
}
