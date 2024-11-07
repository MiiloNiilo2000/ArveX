using Microsoft.Extensions.FileProviders;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using BackEnd.Data;
using BackEnd.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using BackEnd.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey("OurSecurityKeyThatHasToBeSomeWhatOfALongArrayOfLetters"u8.ToArray()),
            ValidIssuer = "http://id.localhost:3000/",
            ValidAudience = "http://localhost:3000/",
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}); 

builder.Services.AddScoped<InvoiceRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors("AllowAll");

app.MapPost("/login", (LoginRequest request, TokenGenerator tokenGenerator) =>
{
    return new
    {
        access_token = tokenGenerator.GenerateToken(request.Email)
    };
});

app.Run();