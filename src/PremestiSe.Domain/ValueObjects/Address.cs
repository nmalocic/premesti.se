namespace PremestiSe.Domain.ValueObjects;

public record Address
{
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;

    public Address() { }

    public Address(string street, string number, string postalCode)
    {
        Street = street;
        Number = number;
        PostalCode = postalCode;
    }

    public override string ToString() => $"{Street} {Number}, {PostalCode}";
}
