using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using production_Manassas.Dto.SendGridDto;
using Project_Manassas.Database;
using Project_Manassas.Model.SendGridModel;
using Project_Manassas.Service;



namespace Project_Manassas.Controller;

[ApiController]
[Route(ApiEndPoints.SendGridEmail.Base)]

public class SendGridController: ControllerBase
{
    private readonly ISendGridService _sendGridService;
    private readonly IWebHostEnvironment _environment;
    private readonly ProjectContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public SendGridController(ISendGridService sendGridService, IWebHostEnvironment environment, UserManager<IdentityUser> userManager, ProjectContext dbContext)
    {
        _sendGridService = sendGridService;
        _environment = environment;
        _userManager = userManager;
        _dbContext = dbContext;
    }
    
    // POST/api/users/registerSendGrid
    [HttpPost(ApiEndPoints.SendGridEmail.REGISTER_URL_SENDGRID)]
    public async Task<IActionResult> SendGridRegistration(SendGridDto.RegisterDto registerDto)
    {
        var user = new IdentityUser { UserName = registerDto.Email, Email = registerDto.Email, EmailConfirmed = false };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        
        if (!result.Succeeded) return BadRequest(result.Errors);
        
        // Generate and save verificationCode
        var generateCode = GenerateLetterCode.GenerateVerificationCode();
        _dbContext.VerificationCode.Add(new VerificationCodeModel
        {
            UserId = user.Id,
            VerificationCode = generateCode,
            ExpirationTime = DateTime.Now.AddMinutes(10),
            Used = false

        });
        await _dbContext.SaveChangesAsync();
        
        // Send Email
        var subject = "Verify your email";
        var body = $"<p>Your verification code is: <strong>{generateCode}</strong></p>";
        await _sendGridService.SendEmailAsync(user.Email, subject, body);
        
        return Ok("User was registered. Verification code has been sent.");
    }
    
    // POST/api/users/verifyEmail
    [HttpPost(ApiEndPoints.SendGridEmail.VERIFY_URL_SENDGRID)]
    public async Task<IActionResult> VerifyEmail(SendGridDto.VerifyCodeDto verifyDto)
    {
        var codeRecord = _dbContext.VerificationCode.FirstOrDefault(x => x.UserId == verifyDto.UserId && x.VerificationCode == verifyDto.VerificationCode && !x.Used );
        if (codeRecord == null || codeRecord.ExpirationTime < DateTime.Now)
        {
            return BadRequest("Verification code has expired.");
        }

        try
        {
             var user = await _userManager.FindByIdAsync(verifyDto.UserId);
             if (user != null)
             {
                 user.EmailConfirmed = true;
                 await _userManager.UpdateAsync(user);
             }

             codeRecord.Used = true;
             await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return Ok("Email has been verified.");
    }
    
    // POST/api/users/forgotPassword
    [HttpPost(ApiEndPoints.SendGridEmail.FORGOT_URL_PASSWORD)]
    public async Task<IActionResult> ForgotPassword(SendGridDto.ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user == null) return BadRequest("Email is invalid.");

        var verificationCode = GenerateLetterCode.GenerateVerificationCode();
        _dbContext.PasswordResetToken.Add(new PasswordResetTokenModel
        {
            UserId = user.Id,
            PasswordResetCode = verificationCode,
            ExpirationTime = DateTime.Now.AddMinutes(10),
            Used = false
        });
        
        await _dbContext.SaveChangesAsync();

        var subject = "Password Reset Code";
        var body = $"<p>Your password reset code is: <strong>{verificationCode}</strong></p>";
        
       if (user.Email != null) await _sendGridService.SendEmailAsync(user.Email, subject, body);
        
        return Ok("Password Reset Code has been sent.");
    }
    
    // POST/api/users/resetPassword
    [HttpPost(ApiEndPoints.SendGridEmail.RESET_URL_PASSWORD)]
    public async Task<IActionResult> ResetPassword(SendGridDto.ResetPasswordDto resetPasswordDto)
    {
        var token = _dbContext.PasswordResetToken.FirstOrDefault(x => x.UserId == resetPasswordDto.UserId && x.PasswordResetCode == resetPasswordDto.PasswordResetCode && !x.Used);

        if (token == null || token.ExpirationTime < DateTime.Now)
        {
            return BadRequest("Password reset code has expired.");
        }

        var user = await _userManager.FindByIdAsync(resetPasswordDto.UserId);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordDto.NewPassword);
        
        if (!result.Succeeded) return BadRequest(result.Errors);

        token.Used = true;
        await _dbContext.SaveChangesAsync();
        
        return Ok("Password reset has been sent.");
    }
}