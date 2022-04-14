using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunctionAppMoedas.Data;

public class Cotacao
{
    public int Id { get; set; }
    public string? Sigla { get; set; }
    public DateTime Horario { get; set; }
    [Column(TypeName = "numeric(18,4)")]
    public decimal Valor { get; set; }
}