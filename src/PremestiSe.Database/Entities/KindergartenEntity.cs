using VeloxDB.ObjectInterface;

namespace PremestiSe.Database.Entities;

[DatabaseClass]
public abstract class KindergartenEntity : DatabaseObject
{
    /// <summary>
    /// Administrative region (Управа)
    /// </summary>
    [DatabaseProperty]
    public abstract string Administration { get; set; }

    /// <summary>
    /// Municipality (Општина)
    /// </summary>
    [DatabaseProperty]
    public abstract string Municipality { get; set; }

    /// <summary>
    /// Settlement/Town (Насеље)
    /// </summary>
    [DatabaseProperty]
    public abstract string Settlement { get; set; }

    /// <summary>
    /// Parent institution name (Установа)
    /// </summary>
    [DatabaseProperty]
    public abstract string Institution { get; set; }

    /// <summary>
    /// Location name (Назив)
    /// </summary>
    [DatabaseProperty]
    public abstract string Name { get; set; }

    /// <summary>
    /// Street name
    /// </summary>
    [DatabaseProperty]
    public abstract string Street { get; set; }

    /// <summary>
    /// Street number
    /// </summary>
    [DatabaseProperty]
    public abstract string Number { get; set; }

    /// <summary>
    /// Postal code
    /// </summary>
    [DatabaseProperty]
    public abstract string PostalCode { get; set; }

    /// <summary>
    /// Location type: 0 = Main, 1 = Branch
    /// </summary>
    [DatabaseProperty]
    public abstract int LocationType { get; set; }
}
