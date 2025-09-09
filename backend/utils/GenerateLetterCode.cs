namespace Project_Manassas;

public class GenerateLetterCode
{
    public static string GenerateVerificationCode(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var randomRange = new Random();
        return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[randomRange.Next(s.Length)]).ToArray());
    }
}