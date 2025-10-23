using Microsoft.EntityFrameworkCore;
using GeneralAPI.Data;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<PlatformAlphaContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformAlphaContext") ?? throw new InvalidOperationException("Connection string 'PlatformAlphaContext' not found.")));

// Add context settings for Render PostgresSQL database
string? connectionString = builder.Configuration.GetConnectionString("RenderPlatformXContext");

builder.Services.AddDbContext<RenderPlatformXContext>(options =>
    options.UseNpgsql(
        connectionString,
        // The first lambda sets Npgsql-specific options (Retry Logic)
        npgsqlOptions =>
        {
            // 1. Enable Retry Logic (Good for remote/cloud connections)
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, 
                maxRetryDelay: TimeSpan.FromSeconds(30), 
                errorCodesToAdd: null
            );

            // 2. Set Query Splitting Behavior (Good for EF performance with collections)
            npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }
    )
);

// Old db context settings
// Include retry settings for when server is asleep and takes long to respond
//builder.Services.AddDbContext<PlatformXContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("RenderPlatformXContext") 
//    ?? throw new InvalidOperationException("Connection string 'RenderPlatformXContext' not found."),
//    sqlOptions =>
//        {
//            sqlOptions.EnableRetryOnFailure(
//                maxRetryCount: 5,        // The maximum number of times to retry (e.g., 5 times)
//                maxRetryDelay: TimeSpan.FromSeconds(30), // The maximum delay (e.g., 30 seconds)
//                errorNumbersToAdd: null   // Use the default list of transient SQL error codes
//            );
//        }
//    ));
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
