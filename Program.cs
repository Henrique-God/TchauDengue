using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using TchauDengue.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string conection = Environment.GetEnvironmentVariable("DATABASE_CONECTION_STRING");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(conection);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
