using CloudinaryDotNet;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Ordernary.Data;
using Ordernary.Repositories.Implementation;
using Ordernary.Repositories.Interface;
using Ordernary.Services.ServiceInterfaces;
using Ordernary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Office.Interop.Word;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHangfire(config => config.UseMemoryStorage());
builder.Services.AddHangfireServer();
// Add services to the container.
builder.Services.AddScoped<IDailyReportService, DailyReportService>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IArticleInterface, ArticleRepository>();
builder.Services.AddScoped<IOrderInterface, OrderRepository>();
builder.Services.AddScoped<IOrderArticleInterface, OrderArticleRepository>();
builder.Services.AddScoped<IReportInterface, ReportRepository>();
builder.Services.AddSingleton<Cloudinary>(sp =>
{
    var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");
    var cloudinaryAccount = new Account(
        cloudinaryConfig["CloudName"],
        cloudinaryConfig["ApiKey"],
        cloudinaryConfig["ApiSecret"]
    );
    return new Cloudinary(cloudinaryAccount);
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localhost",
        ValidAudience = "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("S3DJ8JDNafnhaj812222FAFAFADADADADADASASASASASASA")),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=Ordernary;Trusted_Connection=True;TrustServerCertificate=True;"));

var app = builder.Build();
app.UseHangfireDashboard();
app.UseHangfireServer();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<OrderHub>("/orderHub");

RecurringJob.AddOrUpdate<IDailyReportService>(
    "send-daily-reports",
    service => service.SendDailyReport(),
    Cron.Daily);

app.Run();
