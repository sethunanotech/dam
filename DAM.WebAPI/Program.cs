using DAM.Application;
using DAM.Infrastructure;
using DAM.Persistence;
using DAM.WebAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configuring DB Persistence
builder.Services.ConfigurePersistence(builder.Configuration);

// Configuring Other External Interfaces like Cache, Emails, SMS server etc.,
builder.Services.ConfigureInfrastructure(builder.Configuration);

builder.Services.AddControllers( options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.ConfigureApplication();
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
