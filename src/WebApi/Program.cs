using CleanSSO.Infrastructure.Data;
using CleanSSO.Infrastructure.Repositories;
using CleanSSO.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CleanSSO.Application.Interfaces.IUserRepository, UserRepository>();
builder.Services.AddScoped<CleanSSO.Application.Interfaces.ITokenService, TokenService>();

builder.Services.AddScoped<CleanSSO.Application.Services.AuthService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options => { options.LoginPath = "/account/login"; })
.AddGoogle(options =>
{
    options.ClientId = configuration["Authentication:Google:ClientId"];
    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    options.SaveTokens = true;
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");

    options.Events.OnRemoteFailure = context =>
    {
        if (!string.IsNullOrEmpty(context.Properties?.RedirectUri))
        {
            context.Response.Redirect(context.Properties.RedirectUri);
            context.HandleResponse();
        }
        return Task.CompletedTask;
    };
})
.AddFacebook(options =>
{
    options.AppId = configuration["Authentication:Facebook:AppId"];
    options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
    options.SaveTokens = true;
    options.Fields.Add("picture");

    options.Events.OnRemoteFailure = context =>
    {
        if (!string.IsNullOrEmpty(context.Properties?.RedirectUri))
        {
            context.Response.Redirect(context.Properties.RedirectUri);
            context.HandleResponse();
        }
        return Task.CompletedTask;
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
