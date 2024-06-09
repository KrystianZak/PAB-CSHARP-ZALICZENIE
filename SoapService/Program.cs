using SoapCore;
using ProjektAPI1.ServiceContract; // Upewnij si�, �e ta przestrze� nazw jest prawid�owa
using Microsoft.OpenApi.Models;
using SoapService.ServiceContract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SOAP Core and UserService
builder.Services.AddSoapCore();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Register the SOAP endpoint
app.UseSoapEndpoint<IUserService>("/UserService.asmx", new SoapEncoderOptions());

app.Run();
