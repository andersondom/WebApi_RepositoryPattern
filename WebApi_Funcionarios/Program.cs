using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_ASPNETCore.DataContext;
using WebApi_ASPNETCore.Service.FuncionarioService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFuncionarioInterface, FuncionarioService>();

if (builder.Environment.EnvironmentName != "Testing")
{
    var connectionString =
        builder.Configuration.GetConnectionString("ConexaoPadrao");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "A connection string 'ConexaoPadrao' não foi configurada.");
    }

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });
}

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails =
                new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Erro de validação",
                    Detail =
                        "Um ou mais campos possuem valores inválidos.",
                    Instance =
                        context.HttpContext.Request.Path
                };

            problemDetails.Extensions["traceId"] =
                context.HttpContext.TraceIdentifier;

            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes =
                {
                    "application/problem+json"
                }
            };
        };
    });

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature =
            context.Features.Get<IExceptionHandlerFeature>();

        var exception =
            exceptionHandlerFeature?.Error;

        var logger =
            context.RequestServices
                .GetRequiredService<ILogger<Program>>();

        if (exception is not null)
        {
            logger.LogError(
                exception,
                "Erro não tratado durante a requisição {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
        }

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        context.Response.ContentType =
            "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status =
                StatusCodes.Status500InternalServerError,

            Title =
                "Erro interno do servidor",

            Detail =
                app.Environment.IsDevelopment()
                    ? exception?.Message
                    : "Ocorreu um erro inesperado ao processar a requisição.",

            Instance =
                context.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            context.TraceIdentifier;

        await context.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken:
                context.RequestAborted);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

public partial class Program
{
}
