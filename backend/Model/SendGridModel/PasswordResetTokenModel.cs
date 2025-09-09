namespace Project_Manassas.Model.SendGridModel;

public class PasswordResetTokenModel
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string PasswordResetCode { get; set; } = string.Empty;
    public DateTime ExpirationTime { get; set; }
    public bool Used { get; set; }
}