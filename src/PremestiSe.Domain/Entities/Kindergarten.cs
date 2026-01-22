using PremestiSe.Domain.Enums;
using PremestiSe.Domain.ValueObjects;

namespace PremestiSe.Domain.Entities;

public class Kindergarten
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Administrative region (Управа)
    /// </summary>
    public string Administration { get; private set; } = string.Empty;

    /// <summary>
    /// Municipality (Општина)
    /// </summary>
    public string Municipality { get; private set; } = string.Empty;

    /// <summary>
    /// Settlement/Town (Насеље)
    /// </summary>
    public string Settlement { get; private set; } = string.Empty;

    /// <summary>
    /// Parent institution name (Установа)
    /// </summary>
    public string Institution { get; private set; } = string.Empty;

    /// <summary>
    /// Location name (Назив)
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Physical address
    /// </summary>
    public Address Address { get; private set; } = new();

    /// <summary>
    /// Whether this is a main location or a branch
    /// </summary>
    public LocationType LocationType { get; private set; }

    private Kindergarten() { }

    public static Kindergarten Create(
        string administration,
        string municipality,
        string settlement,
        string institution,
        string name,
        Address address,
        LocationType locationType)
    {
        return new Kindergarten
        {
            Id = Guid.NewGuid(),
            Administration = administration,
            Municipality = municipality,
            Settlement = settlement,
            Institution = institution,
            Name = name,
            Address = address,
            LocationType = locationType
        };
    }

    public void Update(
        string? name = null,
        Address? address = null,
        LocationType? locationType = null)
    {
        if (name is not null) Name = name;
        if (address is not null) Address = address;
        if (locationType.HasValue) LocationType = locationType.Value;
    }
}
