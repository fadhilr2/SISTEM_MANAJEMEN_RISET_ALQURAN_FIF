using API.Middleware;
using Lib.services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger / OpenAPI ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistem Manajemen Riset Al-Quran FIF — API",
        Version = "v1",
        Description = """
            REST API for the Al-Quran Research Management System.
            
            **Authentication**
            Use `POST /api/auth/login` to obtain a session token, then click the
            **Authorize** button and paste the token to unlock protected endpoints.
            
            **Roles**
            | Role        | Permissions                                       |
            |-------------|---------------------------------------------------|
            | visitor     | Browse papers, edit own profile                   |
            | researcher  | visitor + upload / edit own papers                |
            | admin       | researcher + manage users, approve requests       |
            """,
        Contact = new OpenApiContact
        {
            Name = "FIF Research Team",
            Email = "admin@mail.com"
        }
    });

    // Bearer token security definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "opaque",
        In = ParameterLocation.Header,
        Description = "Enter your session token (returned by POST /api/auth/login)."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments (generated from <summary> tags)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// ── CORS (allow any during dev) ───────────────────────────────────────────────
builder.Services.AddCors(o => o.AddPolicy("Dev", p => p
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()));

var app = builder.Build();

app.UseCors("Dev");

// ── Swagger UI ────────────────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FIF API v1");
    options.RoutePrefix = string.Empty; // serve at root "/"
    options.DocumentTitle = "FIF Al-Quran API";
    options.DefaultModelsExpandDepth(-1); // hide "Schemas" section by default
});

// ── Session token middleware ──────────────────────────────────────────────────
app.UseMiddleware<SessionTokenMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
