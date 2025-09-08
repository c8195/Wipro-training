using DoConnect.API.Data;
using DoConnect.API.Helpers;
using DoConnect.API.Hubs;
using DoConnect.API.Middleware;
using DoConnect.API.Models;
using DoConnect.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;




using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
});














// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity configuration
builder.Services.AddIdentity<User, Role>(options =>
{
    // Password settings
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    // Sign in settings
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? throw new Exception("JWT key missing"));


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));   
         options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User", "Admin"));
        

});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{  options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };

    // Configure JWT for SignalR
    options.Events = new JwtBearerEvents
    {
        
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception}");
            return Task.CompletedTask;
        },OnTokenValidated = context =>
        {
            Console.WriteLine($"Token validated for: {context.Principal?.Identity?.Name}");
            var roles = context.Principal?.FindAll(ClaimTypes.Role);
            Console.WriteLine($"Roles: {string.Join(", ", roles?.Select(r => r.Value) ?? new string[0])}");
            return Task.CompletedTask;
        },








        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Services
// builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IAuthService, AuthServiceMinimal>();
builder.Services.AddScoped<INotificationService, NotificationService>();




// SignalR
builder.Services.AddSignalR();

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DoConnect API",
        Version = "v1",
        Description = "A Q&A platform API with file upload and real-time notifications"
    });

    // JWT Authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "Uploads","Images");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    // FileProvider = new PhysicalFileProvider(uploadsPath),
     FileProvider = new PhysicalFileProvider(uploadsPath),
    // Set the request path to /api/images so that the static files in the "uploads" folder
    // can be accessed via the API endpoint, e.g. /api/images/image1.jpg
    RequestPath = "/api/images"
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DoConnect API v1");
        c.RoutePrefix = string.Empty;
    });
}



app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Unhandled exception occurred. Request: {Method} {Path}", 
            context.Request.Method, context.Request.Path);
        
        // Log stack trace
        logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
        
        throw; // Re-throw to let the normal error handling continue
    }
});














// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");


// Serve static files from Uploads directory
app.UseStaticFiles();

app.MapControllers();
app.MapGet("/api/images/{fileName}", (string fileName) =>
{
  var uploadsFolder = Path.Combine(app.Environment.ContentRootPath, "Uploads", "Images");
  var filePath = Path.Combine(uploadsFolder, fileName);

  if (!System.IO.File.Exists(filePath))
    return Results.NotFound();

  // infer content-type
  var provider = new FileExtensionContentTypeProvider();
  if (!provider.TryGetContentType(filePath, out var contentType))
    contentType = "application/octet-stream";

  return Results.File(filePath, contentType);
})
.WithName("GetImage")
.Produces<FileResult>(200)
.Produces(404);

// SignalR Hub
app.MapHub<NotificationHub>("/notificationHub");

// Database initialization
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
      await DbSeeder.SeedAsync(scope.ServiceProvider);
    
    await DbInitializer.Initialize(context, userManager, roleManager);
}



app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();   
app.Run();

