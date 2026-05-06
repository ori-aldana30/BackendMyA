using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

// Railway a veces devuelve postgres://... entonces lo convertimos con NpgsqlConnectionStringBuilder
if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgres://"))
{
    var uri = new Uri(connectionString);

    var userInfo = uri.UserInfo.Split(':');
    var username = userInfo[0];
    var password = userInfo[1];

    var dbName = uri.AbsolutePath.TrimStart('/');

    var npgsqlBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = username,
        Password = password,
        Database = dbName,
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    connectionString = npgsqlBuilder.ToString();
}

builder.Services.AddDbContext<MyAMISContext>(options =>
    options.UseNpgsql(connectionString)
);

// ======================
// CORS
// ======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Railway
builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

// ======================
// ACTIVAR CORS ANTES DE TODO
// ======================
app.UseCors("AllowAll");

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();