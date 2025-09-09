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

    public SendGridController(ISendGridService sendGridService, IWebHostEnvironment environment)
    {
        _sendGridService = sendGridService;
        _environment = environment;
    }
    
    // POST/api/users/registerSendGrid
    [HttpPost(ApiEndPoints.SendGridEmail.REGISTER_URL_SENDGRID)]
    public async Task<IActionResult> SendGridRegistration(SendGridDto.RegisterDto registerDto, UserManager<IdentityUser> userManager,
        [FromServices] SendGridEmailService emailService, ProjectContext dbContext)
    {
        var user = new IdentityUser { UserName = registerDto.Email, Email = registerDto.Email, EmailConfirmed = false };
        var result = await userManager.CreateAsync(user, registerDto.Password);
        
        if (!result.Succeeded) return BadRequest(result.Errors);
        
        // Generate and save verificationCode
        var generateCode = GenerateLetterCode.GenerateVerificationCode();
        dbContext.VerificationCode.Add(new VerificationCodeModel
        {
            UserId = user.Id,
            VerificationCode = generateCode,
            ExpirationTime = DateTime.Now.AddMinutes(10),
            Used = false

        });
        await dbContext.SaveChangesAsync();
        
        // Send Email
        var subject = "Verify your email";
        var body = $"<p>Your verification code is: <strong>{generateCode}</strong></p>";
        await emailService.SendEmailAsync(user.Email, subject, body);
        
        return Ok("User was registered. Verification code has been sent.");
    }
    
    // POST/api/users/verifyEmail
    [HttpPost(ApiEndPoints.SendGridEmail.VERIFY_URL_SENDGRID)]
    public async Task<IActionResult> VerifyEmail(SendGridDto.VerifyCodeDto verifyDto,
        [FromServices] UserManager<IdentityUser> userManager,
        [FromServices] ProjectContext dbContext)
    {
        var codeRecord = dbContext.VerificationCode.FirstOrDefault(x => x.UserId == verifyDto.UserId && x.VerificationCode == verifyDto.VerificationCode && !x.Used );
        if (codeRecord == null || codeRecord.ExpirationTime < DateTime.Now)
        {
            return BadRequest("Verification code has expired.");
        }

        try
        {
             var user = await userManager.FindByIdAsync(verifyDto.UserId);
             user.EmailConfirmed = true;
             await userManager.UpdateAsync(user);

             codeRecord.Used = true;
             await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return Ok("Email has been verified.");
    }
}