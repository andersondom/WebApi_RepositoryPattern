using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_ASPNETCore.DataContext;
using WebApi_ASPNETCore.Service.FuncionarioService;

var builder = WebApplication.CreateBuilder(args);

// Dependências da aplicação
builder.Services.AddScoped<IFuncionarioInterface, FuncionarioService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexaoPadrao"));
});

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value!.Errors
                        .Select(error =>
                            string.IsNullOrWhiteSpace(error.ErrorMessage)
                                ? "Valor inválido."
                                : error.ErrorMessage)
                        .ToArray());

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Um ou mais dados informados são inválidos.",
                Detail = "Verifique os campos indicados e tente novamente.",
                Instance = context.HttpContext.Request.Path
            };

            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes =
                {
                    "application/problem+json"
                }
            };
        };
    });
    
// Padronização de erros HTTP
builder.Services.AddProblemDetails();

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Tratamento global de exceções
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerFeature =
            context.Features.Get<IExceptionHandlerFeature>();

        var exception = exceptionHandlerFeature?.Error;

        var logger = context.RequestServices
            .GetRequiredService<ILogger<Program>>();

        if (exception is not null)
        {
            logger.LogError(
                exception,
                "Erro não tratado durante a requisição {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Ocorreu um erro interno no servidor.",
            Detail = app.Environment.IsDevelopment()
                ? exception?.Message
                : "Não foi possível processar a solicitação.",
            Instance = context.Request.Path
        };

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        context.Response.ContentType =
            "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

// Swagger somente em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}