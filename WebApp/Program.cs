using WebApp.Auth;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<JwtAuthorizationHandler>();
builder.Services.AddScoped<ApiFetchService>();


builder.Services.AddHttpClient("ApiClient", client =>
    client.BaseAddress = new Uri("http://localhost:5020/")
).AddHttpMessageHandler<JwtAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
