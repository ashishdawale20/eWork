namespace CrimePortal.ViewModels
{
    public class AdminResetPasswordViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
