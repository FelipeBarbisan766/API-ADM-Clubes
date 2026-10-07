using API_PI_ADM_Clubes.Model;

public class Manager : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}