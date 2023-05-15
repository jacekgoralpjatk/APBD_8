using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TripApi.Models;
using TripApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ITripService, TripService>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApbdDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();