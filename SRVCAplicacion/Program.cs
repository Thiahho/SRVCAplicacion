using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

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

// Configura la base de datos con MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Configuraci�n de autenticaci�n
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";    
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

// Agregar servicios de logging
builder.Services.AddLogging();

var app = builder.Build();

// Configuraci�n del manejo de errores global
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500; // O el c�digo de error adecuado

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        // Registrar la excepci�n si existe
        if (exception != null)
        {
            logger.LogError(exception, "Ocurri� un error inesperado.");
        }

        var errorResponse = new
        {
            message = "Ocurri� un error inesperado.",
            status = context.Response.StatusCode
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

// Configuraci�n del pipeline de HTTP
if (!app.Environment.IsDevelopment())
{
    // En producci�n, se maneja el error de manera global y se configura HSTS
    app.UseHsts();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Habilitar CORS
app.UseCors("PermitirTodo");

// Configuraci�n de autenticaci�n y autorizaci�n
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Rutas predeterminadas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

//cors
app.UseCors("PermitirTodo");
app.MapControllers();
//fin cors
app.Run();


