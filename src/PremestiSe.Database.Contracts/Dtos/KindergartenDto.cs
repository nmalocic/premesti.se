namespace PremestiSe.Database.Contracts.Dtos;

public class KindergartenDto
{
    public long Id { get; set; }
    public string Administration { get; set; } = string.Empty;
    public string Municipality { get; set; } = string.Empty;
    public string Settlement { get; set; } = string.Empty;
    public string Institution { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public int LocationType { get; set; } // 0 = Main, 1 = Branch
}
