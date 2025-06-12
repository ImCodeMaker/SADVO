using SADVO.Core.Application;
using SADVO.Core.Application.Interfaces;
using SADVO.Infrastructure.Persistence;
using SADVO.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(opt =>
{
	opt.IdleTimeout = TimeSpan.FromMinutes(60);
	opt.Cookie.HttpOnly = true;
	opt.Cookie.IsEssential = true;
});

builder.Services.AddPersistenceLayerIOC(builder.Configuration);
builder.Services.AddServicesLayerIOC(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserSession, UserSession>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();


app.Run();
