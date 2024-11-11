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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.SetIsOriginAllowed(_ => true)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });

builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")))
    .AddScoped<InvoiceRepo>()
    .AddScoped<ProfileRepo>();



var app = builder.Build();

using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
{
    context?.Database.EnsureCreated();
}


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

app.UseCors("MyPolicy");

app.Run();