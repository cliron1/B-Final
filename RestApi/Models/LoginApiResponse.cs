using BezeqFinalProject.Common.Data.Entities;

namespace BezeqFinalProject.WebApi.Models;

public class LoginApiResponse {
    public string Token { get; set; }
    public User User { get; set; }
}
