namespace Fina.Core.Requests.Transactions;

public class GetTransactionsByPeriodRequest : Request
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
