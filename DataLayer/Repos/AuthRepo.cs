using BezeqFinalProject.Common.Data.Contexts;
using BezeqFinalProject.Common.Data.Entities;
using BezeqFinalProject.Common.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BezeqFinalProject.Common.Repos;

public interface IAuthRepo {
    Task<User> Login(LoginRequestModel model);
    Task Logout(IEnumerable<Claim> claims);
    Task<User> Signup(SignupRequestModel model);
}

public class AuthRepo : IAuthRepo {
    private readonly MainContext context;
    private readonly ILogger<AuthRepo> logger;

    public AuthRepo(MainContext context, ILogger<AuthRepo> logger) {
        this.context = context;
        this.logger = logger;
    }

    public async Task<User> Login(LoginRequestModel model) {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email.Equals(model.Email) && x.PwdHash == model.Pwd.Hash());
        return user;
    }

    public async Task Logout(IEnumerable<Claim> claims) {
        // Do something
    }

    public async Task<User> Signup(SignupRequestModel model) {
        model.Email = model.Email.ToLower().Trim();

        var user = await context.Users.SingleOrDefaultAsync(x => x.Email.Equals(model.Email));
        if(user != null)
            throw new Exception("User already exist");

        user = new User {
            Name = model.Name,
            Email = model.Email,
            PwdHash = model.Pwd.Hash()
        };
        await context.Users.AddAsync(user);
        var id = await context.SaveChangesAsync();
        user.Id = id;

        return user;
    }
}
