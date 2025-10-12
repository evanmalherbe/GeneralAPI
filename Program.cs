using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GeneralAPI.Data;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<PlatformAlphaContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformAlphaContext") ?? throw new InvalidOperationException("Connection string 'PlatformAlphaContext' not found.")));

// Include retry settings for when server is asleep and takes long to respond
builder.Services.AddDbContext<PlatformAlphaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformAlphaContext") 
    ?? throw new InvalidOperationException("Connection string 'PlatformAlphaContext' not found."),
    sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,        // The maximum number of times to retry (e.g., 5 times)
                maxRetryDelay: TimeSpan.FromSeconds(30), // The maximum delay (e.g., 30 seconds)
                errorNumbersToAdd: null   // Use the default list of transient SQL error codes
            );
        }
    ));
// Add services to the container.

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

app.UseAuthorization();

app.MapControllers();

app.Run();
