namespace PriceNest.Api.Models;

public class User
{
    public int Id {get; set;}
    public string Login {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;

    public List<UserProduct> ObservedProducts {get; set;} = [];
}