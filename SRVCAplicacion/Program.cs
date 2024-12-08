using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using SRVCAplicacion.Services;

var builder = WebApplication.CreateBuilder(args);

//****************CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//builder.WebHost.UseKestrel(options =>
//{
//    options.ListenLocalhost(5000); 
//    options.ListenLocalhost(5001, listenOptions => listenOptions.UseHttps()); 
//});

// Registrar los servicios
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);  // Cambiar a UseNpgsql
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.LogoutPath = "/Account/Logout"; // Ruta de cierre de sesión
        options.AccessDeniedPath = "/Account/Login"; // Ruta para acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

// Registrar servicios adicionales
builder.Services.AddScoped<ILogAudService, AuditoriaService>();
builder.Services.AddHttpContextAccessor();

// Build the application
var app = builder.Build();

// Configuración del pipeline de solicitudes
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Definir las rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

// Aplicar la política de CORS
app.UseCors("PermitirTodo");

// Mapear controladores
app.MapControllers();

app.Run();
