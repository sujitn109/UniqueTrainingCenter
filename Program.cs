using Microsoft.EntityFrameworkCore;
using UniqueClassesSite.Models;

var builder = WebApplication.CreateBuilder(args);

// Database Connection ॲड करा
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("condb")));

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ३० मिनिटांनंतर सेशन संपेल
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// क्रॅश टाळण्यासाठी एरर हँडलर
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Somee वरील HTTPS एरर टाळण्यासाठी हे फक्त लोकल मशीनवर चालेल असे केले आहे
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseDefaultFiles(); // 👈 ही ओळ नवीन जोडली आहे (स्टार्टअप व्यवस्थित होण्यासाठी)
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // 👈 ही ओळ अत्यंत महत्त्वाची आहे! (AddSession वापरल्यामुळे ही लागतेच)

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();