using GerenciadorUsuario.Filters;
using GerenciadorUsuario.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());
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

// Middleware de log de requisições
app.Use((httpContext, next) => 
{
    var logger = httpContext.RequestServices.GetService<ILogger<Program>>();
    logger.LogInformation(
        "Requisição com o método {Metodo} para rota {Rota}", 
        httpContext.Request.Method, 
        httpContext.Request.Path);
    
    return next();
});

app.Run();