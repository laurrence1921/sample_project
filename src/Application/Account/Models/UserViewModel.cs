using System.Collections.Generic;

namespace CleanArchitecture.Application.Account.Models;

public class UserViewModel
{
    public string Id { get; set; }
    public string AccessToken { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PhotoUrl { get; set; }
    public string RoleName { get; set; }
}
