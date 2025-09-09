namespace production_Manassas.Dto.SendGridDto;

public class SendGridDto
{
    public class RegisterDto
    {
        public required string Email { get; set; }
       
        public required string Password { get; set; }
    }

    public class VerifyCodeDto
    {
        public required string UserId {get; set;}
        
        public required string VerificationCode {get; set;}
    }

    public class ForgotPasswordDto
    {
        public required string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public required string UserId { get; set; }
        public required string PasswordResetCode { get; set; }
        public required string NewPassword { get; set; }
    }
    
    
}