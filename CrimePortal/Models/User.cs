namespace CrimePortal.Models
{
    public class User
    {
        public int UserId { get; set; }          // Primary key
        public string UserName { get; set; }     // Username (English)
        public string PasswordHash { get; set; }      // Hashed password
        public string UserType { get; set; }     // Admin / User

        public bool IsActive { get; set; } = true; 
    }
}
