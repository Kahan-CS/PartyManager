using Microsoft.EntityFrameworkCore;
using PartyManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

string? dbConn = builder.Configuration.GetConnectionString("PartiesKDesai6826");
if (dbConn == null)
{
    Console.WriteLine("Connection string not found");
    return;
}
builder.Services.AddDbContext<PartyDbContext>(options => options.UseSqlServer(dbConn));

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
