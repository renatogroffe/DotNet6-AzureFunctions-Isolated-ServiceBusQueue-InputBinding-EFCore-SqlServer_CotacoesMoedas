using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FunctionAppMoedas.Data;
using FunctionAppMoedas.Models;
using FunctionAppMoedas.Validations;

namespace FunctionAppMoedas;

public class ProcessarCotacoes
{
    private readonly ILogger _logger;
    private readonly CotacoesRepository _repository;

    public ProcessarCotacoes(ILoggerFactory loggerFactory,
        CotacoesRepository repository)
    {
        _logger = loggerFactory.CreateLogger<ProcessarCotacoes>();
        _repository = repository;
    }

    [Function(nameof(ProcessarCotacoes))]
    public void Run([ServiceBusTrigger("queue-dolar",
        Connection = "AzureServiceBusConnection")] DadosCotacao dadosCotacao)
    {
        _logger.LogInformation(
            $"Dados recebidos: {JsonSerializer.Serialize(dadosCotacao)}");

        var validationResult = new DadosCotacaoValidator().Validate(dadosCotacao);
        if (validationResult.IsValid)
        {
            _repository.Save(dadosCotacao);
            _logger.LogInformation("Cotacao registrada com sucesso!");
        }
        else
        {
            _logger.LogError("Dados invalidos para a Cotacao");
            foreach (var error in validationResult.Errors)
                _logger.LogError($" ## {error.ErrorMessage}");
        }
    }
}