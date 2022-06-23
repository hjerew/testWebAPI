using Microsoft.EntityFrameworkCore;
using testWebAPI.Models;
using testWebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<CountryContext>(opt =>
    opt.UseInMemoryDatabase("CountryList"));
builder.Services.AddHttpClient<testWebAPI.Controllers.FirebaseClient>((client =>
{
    client.BaseAddress = new System.Uri("https://fir-functions-api-430be-default-rtdb.firebaseio.com/.json");
}));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
