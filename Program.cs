using System.Text;
using GerenciadorUsuario.Filters;
using GerenciadorUsuario.Repository;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication().AddJwtBearer(options => 
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "usuario-api",
        ValidAudience = "usuario-api",
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("ChaveAutenticacao"))),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("buscar-por-id", policy => policy.RequireClaim("ler-dados-por-id"));
});
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

/*var configuration = builder.Configuration;
var connectionString = configuration["ConnectionStrings:ChaveAutenticacao"];
var apiKey = configuration["ChaveAutenticacao"];*/

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