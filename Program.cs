using System.Reflection;
using System.Text;
using Asp.Versioning;
using GerenciadorUsuario.Filters;
using GerenciadorUsuario.Repository;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddRateLimiter(_ =>
{
    _.AddFixedWindowLimiter("janela-fixa", options =>
    {
        options.QueueLimit = 5;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.PermitLimit = 2;
        options.Window = TimeSpan.FromSeconds(5);
    });
    
});   
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
builder.Services.AddSwaggerGen( options => 
{
    var documentacao = new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "usuarioadmin@gmail.com",
            Name = "Equipe APi",
            Url = new Uri("https://github.com/Siqueiraaf/GerenciadorUsuario")
        },
        Description = "API para gerenciamento de usuários",
        Title = "Gerenciador de Usuários",
    };

    options.SwaggerDoc("v1", documentacao);
    options.SwaggerDoc("v2", documentacao);

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddApiVersioning(options => 
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

}).AddApiExplorer(options => 
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        foreach (var apiVersion in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint($"/swagger/{apiVersion.GroupName}/swagger.json", apiVersion.GroupName);
        }
    });
}

/*var configuration = builder.Configuration;
var connectionString = configuration["ConnectionStrings:ChaveAutenticacao"];
var apiKey = configuration["ChaveAutenticacao"];*/
app.UseRateLimiter();

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