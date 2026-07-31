using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorFinanceiro.Api.ErrorHandling;

/// <summary>
/// Converte exceções não tratadas em respostas HTTP 500 padronizadas.
/// </summary>
/// <remarks>
/// Registra o erro completo nos logs e retorna ao cliente apenas informações
/// seguras, incluindo um identificador que permite localizar a falha.
/// </remarks>
public sealed class ApiExceptionHandler(
    ILogger<ApiExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    /// <summary>
    /// Trata uma exceção inesperada e gera uma resposta Problem Details.
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Reutiliza o identificador da atividade ou o da própria requisição.
        var traceId =
            Activity.Current?.Id ??
            httpContext.TraceIdentifier;

        // Log estruturado para facilitar a localização e o diagnóstico do erro.
        logger.LogError(
            exception,
            "Erro não tratado durante {MetodoHttp} {Caminho}. TraceId: {TraceId}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            traceId);

        httpContext.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        // Não expõe stack trace ou detalhes internos ao cliente.
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Ocorreu um erro inesperado.",
            Detail =
                "Não foi possível processar a solicitação. " +
                "Utilize o identificador do erro caso precise de suporte.",
            Instance = httpContext.Request.Path
        };

        // Relaciona a resposta recebida ao erro registrado nos logs.
        problemDetails.Extensions["traceId"] = traceId;

        var foiEscrito = await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });

        if (foiEscrito)
        {
            return true;
        }

        // Fallback caso nenhum writer configurado aceite o formato solicitado.
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}