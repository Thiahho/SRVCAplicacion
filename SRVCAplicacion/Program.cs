//using Microsoft.EntityFrameworkCore;
//using SRVCAplicacion.Data;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using SRVCAplicacion.Services;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//////builder.WebHost.UseKestrel(options =>
//////{
//////    options.ListenAnyIP((500));
//////    options.ListenAnyIP(5001,listenOPtions =>
//////    {
//////        listenOPtions.UseHttps();
//////    });
//////});

////// Add services to the container.

//////****************CORS

//////var builderC = WebApplication.CreateBuilder(args);

////Configura CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("PermitirTodo", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});
////Agregar servicios de controladores
//builder.Services.AddControllers();
//builder.Services.AddScoped<ILogAudService, AuditoriaService>();

//var jwtSettings = builder.Configuration.GetSection("Jwt");
//var key= Encoding.UTF8.GetBytes(jwtSettings["Key"]!);



//////var appC = builderC.Build();

//////Aplicar la politica de CORS
//////appC.UseCors("PermitirTodo");

//////appC.UseAuthorization();

//////appC.MapControllers();

//////appC.Run();

////*************FIN CORS



//builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//    options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.EnableRetryOnFailure());
//});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata = false; // Deshabilitar para desarrollo, en producción usar HTTPS
//        options.SaveToken = true;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Emisor del token
//            ValidAudience = builder.Configuration["Jwt:Audience"], // Audiencia permitida
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Clave secreta
//        };
//    });


//builder.Services.AddScoped<ILogAudService, AuditoriaService>();
//builder.Services.AddHttpContextAccessor();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Acceso}/{action=Login}/{id?}");

////cors
//app.UseCors("PermitirTodo");
//app.MapControllers();
////fin cors
//app.Run();


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SRVCAplicacion.Data;
using SRVCAplicacion.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Agregar DbContext (PostgreSQL en este caso)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.EnableRetryOnFailure());
});

// Configuración de autenticación JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Deshabilitar para desarrollo, en producción usar HTTPS
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Registrar servicios adicionales
builder.Services.AddScoped<ILogAudService, AuditoriaService>(); 
builder.Services.AddHttpContextAccessor();

// Agregar controladores (y vistas si es necesario)
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.Urls.Add("http://localhost:7285");
// Configuración del pipeline de la aplicación
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Aplicar la política de CORS
app.UseCors("PermitirTodo");

// Configurar las rutas de los controladores
app.MapControllers();

app.Run();
