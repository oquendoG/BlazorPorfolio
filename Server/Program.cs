using Microsoft.EntityFrameworkCore;
using Serilog;
using Server.Data;
using Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

//configuramos el logger
var loggger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(loggger);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options
                .UseSqlite(builder.Configuration.GetConnectionString("conexion")));

builder.Services.AddMapsterConfigs();
builder.Services.AddMediatrConfigs();
builder.Services.ConfigureCors();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WilsonServer API");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("corspolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
