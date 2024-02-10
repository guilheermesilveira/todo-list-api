using TodoList.API.Configuration;
using TodoList.Application;
using TodoList.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationConfig(builder.Configuration);

builder.Services.AddInfraDataConfig(builder.Configuration);

builder.Services.AddSwaggerConfig();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("default");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();