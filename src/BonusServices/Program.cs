using Microsoft.EntityFrameworkCore;
using BonusServices.DataAcess;
using BonusServices.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BonusDBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddTransient<IPrivilegeDA, PrivilegeDA>();
builder.Services.AddTransient<PrivilegeService>();
builder.Services.AddTransient<IPrivilegeHistoryDA, PrivilegeHistoryDA>();
builder.Services.AddTransient<PrivilegeHistoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();