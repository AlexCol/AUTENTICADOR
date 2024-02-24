using AUTENTICADOR.src.Extensions.toBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.addDependencies();


var app = builder.Build();

//!mapear o uso do swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{"Rest Api para autenticação"}"));

app.UseHttpsRedirection();
app.MapControllers();
app.Run();