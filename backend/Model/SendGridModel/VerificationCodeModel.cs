namespace Project_Manassas.Model.SendGridModel;

public class VerificationCodeModel
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
    public DateTime ExpirationTime { get; set; }
    public bool Used { get; set; }
}