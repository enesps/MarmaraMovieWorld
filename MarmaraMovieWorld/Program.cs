using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Pages;
using MarmaraMovieWorld.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddScoped<TMDbService>();
builder.Services.AddHttpClient();
builder.Services.Configure<ApiKeysOptions>(builder.Configuration.GetSection("ApiKeys")); // Yapýlandýrmayý ekleyin
builder.Services.AddScoped<MovieDetailModel>();

// Configuration
var configuration = builder.Configuration;

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
