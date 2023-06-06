using BezeqFinalProject.Common.Data.Contexts;
using BezeqFinalProject.Common.Repos;
using BezeqFinalProject.Common.Services;
using BezeqFinalProject.WebApi.config;
using BezeqFinalProject.WebApi.Config;
using Microsoft.AspNetCore.Cors.Infrastructure;
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

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.AddResponseCompression();

builder.Services.AddAuth(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddSwagger();

var corsAllowedUrls = builder.Configuration.GetSection("Jwt:AllowedOrigins").Get<List<string>>();
builder.Services.AddCors(options => {
    options.AddPolicy("client-sample",
        builder => builder
            .WithOrigins(corsAllowedUrls.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        );
});






var app = builder.Build();

await using(var scope = app.Services.CreateAsyncScope()) {
    using var db = scope.ServiceProvider.GetService<MainContext>();
    //await db!.Database.MigrateAsync();
    await db.Database.EnsureCreatedAsync();
}

if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseCors("client-sample");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();
