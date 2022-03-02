namespace BlazorGameDotNet6.Shared
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? Bio { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Coins { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool isConfirmed { get; set; }
        public bool isDeleted { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        // one user can have multiple units
        public List<UserUnit> UserUnits { get; set; }
        public int Battles { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
