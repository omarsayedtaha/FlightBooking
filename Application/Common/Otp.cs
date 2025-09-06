using System.Security.Cryptography.X509Certificates;

public class Otp
{
    public string Code { get; set; }

    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(5);

    public bool IsValid() => DateTime.Now < ExpiresAt;

    public bool IsUsed { get; set; } = false;
}