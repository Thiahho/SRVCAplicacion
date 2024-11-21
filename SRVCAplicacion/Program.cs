//using Microsoft.EntityFrameworkCore;
//using SRVCAplicacion.Data;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Diagnostics;
//using System.Text.Json;
//using SRVCAplicacion.Service;

//var builder = WebApplication.CreateBuilder(args);


////Configura CORS


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("PermitirTodo", policy =>
//    {
//        policy.WithOrigins("https://localhost:7285")
//              .AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});
//builder.Services.AddControllers()
//                .AddJsonOptions(options =>
//                {
//                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//                });

////var appC = builderC.Build();

////Aplicar la politica de CORS
////appC.UseCors("PermitirTodo");

////appC.UseAuthorization();

////appC.MapControllers();

////appC.Run();

////*************FIN CORS



//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

////builder.Services.AddControllersWithViews();


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Acceso/Login";    
//        options.LogoutPath= "/Acceso/Logout";
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//    });
//builder.Services.AddControllersWithViews();
////Service de LogAud
//builder.Services.AddScoped<ILogAudService, LogAudService>();
//// Agregar servicios de logging
//builder.Services.AddLogging();

//var app = builder.Build();


//// Configuraci�n del manejo de errores global
//app.UseExceptionHandler(errorApp =>
//{
//    errorApp.Run(async context =>
//    {
//        context.Response.ContentType = "application/json";
//        context.Response.StatusCode = 500; // O el c�digo de error adecuado

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

//        // Registrar la excepci�n si existe
//        if (exception != null)
//        {
//            logger.LogError(exception, "Ocurri� un error inesperado.");
//        }

//        var errorResponse = new
//        {
//            message = "Ocurri� un error inesperado.",
//            status = context.Response.StatusCode
//        };

//        await context.Response.WriteAsJsonAsync(errorResponse);
//    });
//});

//// Configuraci�n del pipeline de HTTP
//if (!app.Environment.IsDevelopment())
//{
//    // En producci�n, se maneja el error de manera global y se configura HSTS
//    app.UseHsts();
//}
//app.UseMiddleware<ExceptionMiddleware>();

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//// Habilitar CORS
//app.UseCors("PermitirTodo");

//// Configuraci�n de autenticaci�n y autorizaci�n
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//// Rutas predeterminadas
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Acceso}/{action=Login}/{id?}");

////cors
//app.UseCors("PermitirTodo");
//app.MapControllers();
////fin cors
//app.Run();


using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configura CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.WithOrigins("https://localhost:7285") // Ajusta según el puerto o dominios correctos
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.IsEssential = true;
});


builder.Services.AddControllersWithViews().
    AddSessionStateTempDataProvider();
// Configuración de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.LogoutPath = "/Acceso/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

// Agregar servicios
//builder.Services.AddScoped<ILogAudService, LogAudService>();
builder.Services.AddLogging();

var app = builder.Build();

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configuración global de manejo de errores
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500; // O el código de error adecuado

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception != null)
        {
            logger.LogError(exception, "Ocurrió un error inesperado.");
        }

        var errorResponse = new
        {
            message = "Ocurrió un error inesperado.",
            status = context.Response.StatusCode
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

// Configuración del pipeline de HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();// Activa HSTS en producción
}

app.UseHttpsRedirection();  // Redirige a HTTPS
app.UseStaticFiles();       // Sirve archivos estáticos

// Habilitar CORS
app.UseCors("PermitirTodo");

// Configuración de autenticación y autorización
app.UseRouting();        // Enrutamiento antes de la autenticación
app.UseAuthentication(); // Middleware de autenticación
app.UseAuthorization();  // Middleware de autorización

// Definir rutas predeterminadas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

// Mapear los controladores
app.MapControllers();

// Ejecutar la aplicación
app.Run();
