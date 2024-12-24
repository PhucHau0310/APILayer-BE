using APILayer.Data;
using APILayer.Hubs;
using APILayer.Middlewares;
using APILayer.Security;
using APILayer.Services.Implementations;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add SignalR service
builder.Services.AddSignalR();

// Configure authentication with JWT Bearer
builder.Services.AddAuthentication(options =>
{
    //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // For JWT
    //    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // For JWT
    //    //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // For Cookie-based sign-in
    //    //options.DefaultChallengeScheme = "Google";
    //     options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // For JWT
    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // For Google
    //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // For Cookie-based sign-in

    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };

    // Add this section to handle WebSocket authentication
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if ((!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub")) || (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationhub")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
})
.AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = configuration["Google:ClientId"];
    options.ClientSecret = configuration["Google:ClientSecret"];
    options.SaveTokens = true;
    options.CallbackPath = "/api/Auth/google-response";
    //options.SaveTokens = true;
});


builder.Services.AddControllersWithViews();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://localhost:7036",
                            "http://localhost:7036",
                            "http://localhost:3000",
                            "https://accounts.google.com") // replace with your frontend port
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});


// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
    options.AddPolicy("ProviderOnly", policy => policy.RequireRole("Provider"));
    options.AddPolicy("Public", policy => policy.RequireAssertion(context => true));
});

builder.Services.AddSingleton<IUserIdProvider, IdentityProvider>();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IFAQService, FAQService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<IFeaturedAPIService, FeaturedAPIService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Layer", Version = "v1" }));

builder.Services.AddSwaggerGen(c =>
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    }));
builder.Services.AddSwaggerGen(c =>
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
            new string[] {}
        }
    }));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Layer v1");
        c.OAuthClientId(configuration["Google:ClientId"]);
        c.OAuthClientSecret(configuration["Google:ClientSecret"]);
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

// Place UseCors before UseAuthentication and UseAuthorization
app.UseCors("AllowAll");

// Add UseRouting before UseAuthentication and UseAuthorization
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RoleMiddleware>();
app.UseCookiePolicy();

// Use UseEndpoints to map hub and controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
    endpoints.MapHub<NotificationHub>("/notificationhub");
    endpoints.MapControllers();

    endpoints.MapMethods("/api/Auth/signin-google", new[] { "OPTIONS" }, context =>
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
        context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
        context.Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7036");
        context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        context.Response.StatusCode = 200;
        return Task.CompletedTask;
    });
});

app.Run();
