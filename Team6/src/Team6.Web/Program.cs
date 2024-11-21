using Microsoft.AspNetCore.Authentication.Cookies;
using Team6.Data.Repositories;
using Team6.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<NoteRepository>();
builder.Services.AddScoped<ReflectionRepository>();
builder.Services.AddScoped<ReflectionDocumentRepository>();
builder.Services.AddScoped<CalendarRepository>();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // session timeout
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        // make cookies non-persisting on logout so that the user is prompted to re-login each time
        options.LoginPath = "/Account/Login";
        options.Cookie.Name = ".Team6.Session";
    });

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();