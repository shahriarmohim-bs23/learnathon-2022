using AutoMapper;
using backend_task3;
using backend_task3.Models;
using backend_task3.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(("MongoDbSettings")));
builder.Services.AddSingleton<Userservices>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowSpecificOrigins",
                         policy =>
                         {
                             policy.WithOrigins("*")
                                             .AllowAnyHeader()
                                                  .AllowAnyMethod();
                         });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();
app.MapControllers();
app.Run();
