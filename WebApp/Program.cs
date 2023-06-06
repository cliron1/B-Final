using BezeqFinalProject.Common.Data.Contexts;
using BezeqFinalProject.Common.Repos;
using BezeqFinalProject.Common.Services;
using BezeqFinalProject.WebApp.Config;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MainContext>(opts =>
    //opts.UseSqlServer(connectionString)
    opts.UseInMemoryDatabase("SampleDB")
);
builder.Services.AddScoped<IAuthRepo, AuthRepo>();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Note: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-7.0
builder.Services.AddAuth();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.AddResponseCompression();

builder.Services.AddRouting(options => options.LowercaseUrls = true);





var app = builder.Build();

await using(var scope = app.Services.CreateAsyncScope()) {
    using var db = scope.ServiceProvider.GetService<MainContext>();
    //await db!.Database.MigrateAsync();
    await db.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
