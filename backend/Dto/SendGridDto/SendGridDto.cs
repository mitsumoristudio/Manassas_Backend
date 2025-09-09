namespace production_Manassas.Dto.SendGridDto;

public class SendGridDto
{
    public class RegisterDto
    {
        public string Email { get; set; }
       
        public string Password { get; set; }
    }

    public class VerifyCodeDto
    {
        public string UserId {get; set;}
        
        public string VerificationCode {get; set;}
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public string UserId { get; set; }
        public string PasswordResetCode { get; set; }
        public string NewPassword { get; set; }
    }
    
    
}