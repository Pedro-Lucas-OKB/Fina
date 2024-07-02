namespace Fina.Core.Requests.Transactions;

public class GetTransactionsByPeriod : Request
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
