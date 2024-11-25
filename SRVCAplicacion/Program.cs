using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using SRVCAplicacion.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseKestrel(options =>
//{
//    options.ListenAnyIP((500));
//    options.ListenAnyIP(5001,listenOPtions =>
//    {
//        listenOPtions.UseHttps();
//    });
//});

// Add services to the container.

//****************CORS

//var builderC = WebApplication.CreateBuilder(args);

//Configura CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//Agregar servicios de controladores
builder.Services.AddControllers();

//var appC = builderC.Build();

//Aplicar la politica de CORS
//appC.UseCors("PermitirTodo");

//appC.UseAuthorization();

//appC.MapControllers();

//appC.Run();

//*************FIN CORS



builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";    
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILogAudService, AuditoriaSerice>();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

//cors
app.UseCors("PermitirTodo");
app.MapControllers();
//fin cors
app.Run();


