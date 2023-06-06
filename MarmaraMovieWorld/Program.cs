using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Pages;
using MarmaraMovieWorld.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarmaraMovieWorld.Data;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("MarmaraMovieWorldContextConnection") ?? throw new InvalidOperationException("Connection string 'MarmaraMovieWorldContextConnection' not found.");

//builder.Services.AddDbContext<MarmaraMovieWorldContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<MarmaraMovieWorldContext>();

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddScoped<TMDbService>();
builder.Services.AddHttpClient();
builder.Services.Configure<ApiKeysOptions>(builder.Configuration.GetSection("ApiKeys")); // Yap�land�rmay� ekleyin
builder.Services.AddScoped<MovieDetailModel>();

// Configuration
var configuration = builder.Configuration;

//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();


app.Run();
