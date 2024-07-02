using System.ComponentModel.DataAnnotations;
using Fina.Core.Enums;

namespace Fina.Core.Requests.Transactions;

public class CreateTransactionRequest : Request
{
    [Required(ErrorMessage = "Título da transação inválido!")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo de transação inválido!")]
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;

    [Required(ErrorMessage = "Valor da transação inválido!")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Categoria inválida!")]
    public long CategoryId { get; set; }

    [Required(ErrorMessage = "Data de transação inválida!")]
    public DateTime? PaidOrReceivedAt { get; set; }
}
