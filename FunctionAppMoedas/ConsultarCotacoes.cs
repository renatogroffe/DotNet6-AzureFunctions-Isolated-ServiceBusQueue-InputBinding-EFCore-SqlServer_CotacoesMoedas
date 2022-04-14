using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using FunctionAppMoedas.Data;
using FunctionAppMoedas.Models;

namespace FunctionAppMoedas;

public class ConsultarCotacoes
{
    private readonly ILogger _logger;
    private readonly CotacoesRepository _repository;

    public ConsultarCotacoes(ILoggerFactory loggerFactory,
        CotacoesRepository repository)
    {
        _logger = loggerFactory.CreateLogger<ConsultarCotacoes>();
        _repository = repository;
    }

    [Function(nameof(ConsultarCotacoes))]
    [OpenApiOperation(operationId: "Cotacoes", tags: new[] { "Cotacoes" }, Summary = "Cotacoes", Description = "Consultar Cotacoes de Moedas Estrangeiras", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<DadosCotacao>), Summary = "Ultimas Cotacoes de Moedas Estrangeiras", Description = "Ultimas Cotacoes de Moedas Estrangeiras")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        _logger.LogInformation("Consultando cotacoes ja cadastradas...");
        
        var dados = _repository.GetAll();
        _logger.LogInformation($"Numero de cotacoes encontradas = {dados.Count()}");
        
        var response = req.CreateResponse();
        response.StatusCode = HttpStatusCode.OK;
        await response.WriteAsJsonAsync(dados);
        return response;
    }
}