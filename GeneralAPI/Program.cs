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

// Configure CORS
builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
  {
    policy.WithOrigins("https://localhost:7223")
    //.WithOrigins("") Add deployed frontend url here
    .AllowAnyHeader()
    .AllowAnyMethod();
  });
});


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

// Commented out for deployment to Render
//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
