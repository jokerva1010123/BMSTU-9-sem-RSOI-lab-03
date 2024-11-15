using Microsoft.EntityFrameworkCore;
using FlightServices.DataAcess;
using FlightServices.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FlightDBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddTransient<IFlightDA, FlightDA>();
builder.Services.AddTransient<FlightService>();
builder.Services.AddTransient<IAirportDA, AirportDA>();
builder.Services.AddTransient<AirportServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();