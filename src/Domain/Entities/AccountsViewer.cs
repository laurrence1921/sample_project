namespace CleanArchitecture.Domain.Entities;

public class AccountsViewer : BaseAuditableEntity
{
    public string AccountName { get; set; }
    public decimal Balance { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}
