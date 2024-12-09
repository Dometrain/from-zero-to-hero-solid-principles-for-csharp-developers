using Store.Application.Infrastructure;
using Store.Api.Infrastructure;
using Store.Api.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidators();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var jwtConfig = builder.Configuration.GetJwtConfig();
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddJwtAuthentication(jwtConfig);
builder.Services.AddAuthorization();
builder.Services.AddTransient<TokenService>();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStatusCodePages();
app.Run();
